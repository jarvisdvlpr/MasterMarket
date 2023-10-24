using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewMasterMarket.DAL;
using NewMasterMarket.Models;
using NewMasterMarket.ViewModels;
using Newtonsoft.Json;

namespace NewMasterMarket.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;

        public CheckoutController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies["name"] == null)
            {
                return RedirectToAction("addclient");
            }
            List<BasketCountModel> realItems = new List<BasketCountModel>();
            string itemIdsStr = HttpContext.Request.Cookies["basket"];
            List<BasketItemViewModel> items = new List<BasketItemViewModel>();

            if (!string.IsNullOrWhiteSpace(itemIdsStr))
            {
                items = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(itemIdsStr);
            }

            foreach (var item in items)
            {
                Item myItem = _context.Items.Where(x => x.IsDeleted == false).Include(x => x.Company).FirstOrDefault(x => x.Id == item.ItemId);
                if (myItem != null)
                {
                    BasketCountModel basketCount = new BasketCountModel()
                    {
                        ItemModel = myItem,
                        Count = item.Count
                    };
                    realItems.Add(basketCount);
                }
            }
            string name = HttpContext.Request.Cookies["name"].ToString();
            Client  client = _context.Clients.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.Fullname == name);
            //tut napishi potom yesli ne napisal esho dannie otpravki shtobi napisal
            CheckoutFrontViewModel checkout = new CheckoutFrontViewModel()
            {
                CategoryList = _context.Categories.Where(x => x.IsDeleted == false).ToList(),
                Items = realItems,
                BasketItems = items,
                CurrentClient = client
            };
            return View(checkout);
        }


        //Тут есть кнопка "продолжить покупку" доделай его так чтобы если он уже ввел данные в чекаут отправил, если нет модал бокс показывал чтобы ввести данные
        public IActionResult Cart()
        {
            List<Item> realItems = new List<Item>();
            string itemIdsStr = HttpContext.Request.Cookies["basket"];
            List<BasketItemViewModel> items = new List<BasketItemViewModel>();
            List<CartItemCountViewModel> cartItems = new List<CartItemCountViewModel>();

            if(!string.IsNullOrWhiteSpace(itemIdsStr))
            {
                items = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(itemIdsStr);
            }

            foreach (var item in items)
            {
                Item myItem = _context.Items.Where(x => x.IsDeleted == false).Include(x=>x.Company).FirstOrDefault(x => x.Id == item.ItemId);
                if(myItem != null)
                {
                    CartItemCountViewModel cartItem = new CartItemCountViewModel()
                    {
                        MyItem = myItem,
                        Count = item.Count
                    };
                    cartItems.Add(cartItem);
                }
                    
            }


            CartFrontViewModel cartFront = new CartFrontViewModel()
            {
                CategoryList = _context.Categories.Where(x => x.IsDeleted == false).ToList(),
                Items = cartItems
            };
            return View(cartFront);
        }

        public IActionResult DeleteBasket(int id)
        {
            if (!_context.Items.Where(x => x.IsDeleted == false).Any(x => x.Id == id && !x.IsDeleted))
                return NotFound();

            string itemIdsStr = HttpContext.Request.Cookies["basket"];
            List<BasketItemViewModel> items = new List<BasketItemViewModel>();

            if (!string.IsNullOrWhiteSpace(itemIdsStr))
            {
                items = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(itemIdsStr);
            }

            BasketItemViewModel item = items.FirstOrDefault(x => x.ItemId == id);

            if (item == null)
            {
                return NotFound();
            }

            items.Remove(item);

            itemIdsStr = JsonConvert.SerializeObject(items);
            HttpContext.Response.Cookies.Append("basket", itemIdsStr);

            return RedirectToAction("cart");
        }


        //dopishi
        public IActionResult Ordered()
        {
            Client client = _getClient();
            if (client == null) return RedirectToAction("cart");

            List<Item> cookieItems = new List<Item>();

            string itemIdsStr = HttpContext.Request.Cookies["basket"];
            List<BasketItemViewModel> items = new List<BasketItemViewModel>();

            if (!string.IsNullOrWhiteSpace(itemIdsStr))
            {
                items = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(itemIdsStr);
            }

            foreach (var item in items)
            {
                Item myItem = _context.Items.Where(x => x.IsDeleted == false).Include(x => x.Company).FirstOrDefault(x => x.Id == item.ItemId);
                if (myItem != null)
                    cookieItems.Add(myItem);
            }

            Order order = new Order()
            {
                Code = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                Client = client,
                Items = cookieItems
            };

            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction("index", "home");
        }

        private Client _getClient()
        {
            string name = HttpContext.Request.Cookies["name"].ToString();
            Client client = _context.Clients.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.Fullname == name);
            if (client == null)
            {
                return null;
            }
            return client;
        }

        public IActionResult AddClient()
        {
            AddClientViewModel addClientView = new AddClientViewModel()
            {
                CategoryList = _context.Categories.Where(x => x.IsDeleted == false).ToList(),
                MyClient = new Client()
            };
            return View(addClientView);
        }

        //public IActionResult Login()
        //{
        //    return View();
        //}

        [HttpPost]
        public IActionResult AddClient(AddClientViewModel client)
        {
            AddClientViewModel addClientView = new AddClientViewModel()
            {
                CategoryList = _context.Categories.Where(x => x.IsDeleted == false).ToList(),
                MyClient = client.MyClient
            };
            //if (!ModelState.IsValid)
            //{
            //    return View(addClientView);
            //}

            string upperCasedAdress = client.MyClient.Address.ToLower();

            if (client.MyClient.Number[0] != '+')
            {
                ModelState.AddModelError("MyClient.Number", "Пример номера +7**********");
                return View(addClientView);
            }

            // Proverka dney
            if (upperCasedAdress.Contains("бердск") || upperCasedAdress.Contains("первомайский") || upperCasedAdress.Contains("академгородок"))
            {
                ModelState.AddModelError("MyClient.Address", "Error Вторник пятница");
                return View(addClientView);
            }
            if (upperCasedAdress.Contains("чик") || upperCasedAdress.Contains("прокудка"))
            {
                ModelState.AddModelError("MyClient.Address", "Error Среда суббота");
                return View(addClientView);
            }
            if (upperCasedAdress.Contains("ордынка"))
            {
                ModelState.AddModelError("MyClient.Address", "Error только среда");
                return View(addClientView);
            }
            if (upperCasedAdress.Contains("кудряши") || upperCasedAdress.Contains("рыбачий") || upperCasedAdress.Contains("колывань"))
            {
                ModelState.AddModelError("MyClient.Address", "Error понедельник четверг");
                return View(addClientView);
            }

            HttpContext.Response.Cookies.Append("name", client.MyClient.Fullname);
            _context.Clients.Add(client.MyClient);
            _context.SaveChanges();

            return RedirectToAction("index");
        }


        public IActionResult EditClient()
        {
            Client client = _getClient();
            if (client == null) return NotFound();
          
            AddClientViewModel addClientView = new AddClientViewModel()
            {
                CategoryList = _context.Categories.Where(x => x.IsDeleted == false).ToList(),
                MyClient = client
            };
            TempData["id"] = client.Id;

            return View(addClientView);
        }

        [HttpPost]
        public IActionResult EditClient(AddClientViewModel client)
        {
            AddClientViewModel addClientView = new AddClientViewModel()
            {
                CategoryList = _context.Categories.Where(x => x.IsDeleted == false).ToList(),
                MyClient = client.MyClient
            };
            string upperCasedAdress = client.MyClient.Address.ToLower();

            if (client.MyClient.Number[0] != '+')
            {
                ModelState.AddModelError("MyClient.Number", "Пример номера +7**********");
                return View(addClientView);
            }

            // Proverka dney
            if (upperCasedAdress.Contains("бердск") || upperCasedAdress.Contains("первомайский") || upperCasedAdress.Contains("академгородок"))
            {
                ModelState.AddModelError("MyClient.Address", "Error Вторник пятница");
                return View(addClientView);
            }
            if (upperCasedAdress.Contains("чик") || upperCasedAdress.Contains("прокудка"))
            {
                ModelState.AddModelError("MyClient.Address", "Error Среда суббота");
                return View(addClientView);
            }
            if (upperCasedAdress.Contains("ордынка"))
            {
                ModelState.AddModelError("MyClient.Address", "Error только среда");
                return View(addClientView);
            }
            if (upperCasedAdress.Contains("кудряши") || upperCasedAdress.Contains("рыбачий") || upperCasedAdress.Contains("колывань"))
            {
                ModelState.AddModelError("MyClient.Address", "Error понедельник четверг");
                return View(addClientView);
            }
            //Client realClient = _context.Clients.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.Id == Convert.ToInt32(TempData["id"]));
            //if (realClient == null)
            //{
            //    return NotFound();
            //}
            //realClient = client.MyClient;
            //realClient.Id = Convert.ToInt32(TempData["id"]);
            HttpContext.Response.Cookies.Append("name", client.MyClient.Fullname);
            _context.Clients.Update(client.MyClient);
            _context.SaveChanges();

            return RedirectToAction("editclient");

        }
    }
}
