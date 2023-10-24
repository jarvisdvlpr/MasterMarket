using Microsoft.AspNetCore.Mvc;
using NewMasterMarket.DAL;
using NewMasterMarket.Models;
using NewMasterMarket.ViewModels;
using Newtonsoft.Json;
using System.Diagnostics;

namespace NewMasterMarket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel
            {
                Categories = _context.Categories.Where(x => x.IsDeleted == false).ToList(),
                Items = _context.Items.Where(x=>x.IsDeleted == false).Where(x=>x.Imgsrc != "").Take(12).ToList()
            };
            //return RedirectToAction("ComingSoon");
            return View(homeViewModel);
        }
        

        
        public IActionResult ComingSoon()
        {
            if (HttpContext.Request.Cookies["subscribe"] == "true")
                ViewBag.Sub = "true";
            return View();
        }

        [HttpPost]
        public IActionResult EmailAdd(EmailAddViewModel mail)
        {
            if (!ModelState.IsValid)
                return View("ComingSoon");

            if (_context.Subscribers.FirstOrDefault(x => x.Email == mail.Email) != null)
            {
                ModelState.AddModelError("Email", "Этот эмейл уже подписан на нас");
                return View("ComingSoon");
            }
                

            Subscriber subscriber = new Subscriber { Email = mail.Email };
            _context.Subscribers.Add(subscriber);
            _context.SaveChanges();
            HttpContext.Response.Cookies.Append("subscribe", "true");

            return RedirectToAction("ComingSoon");
        }


        public IActionResult AddBasket(int id)
        {
            Item dataItem = _context.Items.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.Id == id);
            if (dataItem == null)
            {
                return NotFound();
            }
                

            string itemIdsStr = HttpContext.Request.Cookies["basket"];
            List<BasketItemViewModel> items = new List<BasketItemViewModel>();

            if (!string.IsNullOrWhiteSpace(itemIdsStr))
            {
                items = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(itemIdsStr);
            }

            BasketItemViewModel item = items.FirstOrDefault(x => x.ItemId == id);
            if (item == null)
            {
                item = new BasketItemViewModel { ItemId = id, Count = dataItem.MinAmount };
                items.Add(item);
            }
            else
            {
                item.Count += dataItem.MinAmount;
            }

            itemIdsStr = JsonConvert.SerializeObject(items);
            HttpContext.Response.Cookies.Append("basket", itemIdsStr);


            return PartialView("_BasketPartial", _getBasket(items));

        }

        private BasketViewModel _getBasket(List<BasketItemViewModel> basketItems)
        {
            BasketViewModel BasketVM = new BasketViewModel
            {
                BasketItems = new List<ProductBasketItemViewModel>(),
                TotalPrice = 0
            };

            //List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
            //var basketStr = HttpContext.Request.Cookies["basket"];
            //if (!string.IsNullOrWhiteSpace(basketStr))
            //    basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketStr);

            foreach (var item in basketItems)
            {
                Item item1 = _context.Items.Where(x=>x.IsDeleted==false).FirstOrDefault(x => x.Id == item.ItemId);
                ProductBasketItemViewModel productBasketItem = new ProductBasketItemViewModel
                {
                    Item = item1,
                    ItemCount = item.Count
                };

                BasketVM.BasketItems.Add(productBasketItem);
                BasketVM.TotalPrice += item1.Price * item.Count;
            }

            return BasketVM;
        }

        public IActionResult DeleteBasketItem(int id)
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


            return PartialView("_BasketPartial", _getBasket(items));

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AddKoki()
        {
            HttpContext.Response.Cookies.Append("koki", "true");

            return RedirectToAction("index");
        }
    }
}