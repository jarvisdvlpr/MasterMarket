using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewMasterMarket.DAL;
using NewMasterMarket.Models;
using NewMasterMarket.ViewModels;
using Newtonsoft.Json;

namespace NewMasterMarket.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int id)
        {
            Item item = _context.Items.Where(x => x.IsDeleted == false).Include(x=>x.Company).FirstOrDefault(x => x.Id == id);
            if (item == null) return RedirectToAction("error", "home");

            FrontProductViewModel productViewModel = new FrontProductViewModel()
            {
                myItem = item,
                CategoryList = _context.Categories.Where(x=>x.IsDeleted == false).ToList(),
                ItemBasketCount = _getProduct(id)
            };
            ViewBag.Id = productViewModel.myItem.Id;
            ViewBag.AddAmount = productViewModel.myItem.MinAmount;
            return View(productViewModel);
        }

        private int _getProduct(int id)
        {
            //if (!_context.Items.Where(x => x.IsDeleted == false).Any(x => x.Id == id && !x.IsDeleted))
            //    return 0;

            string itemIdsStr = HttpContext.Request.Cookies["basket"];
            List<BasketItemViewModel> items = new List<BasketItemViewModel>();

            if (string.IsNullOrWhiteSpace(itemIdsStr))
            {
                return 0;
            }
            items = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(itemIdsStr);
            BasketItemViewModel item = items.FirstOrDefault(x => x.ItemId == id);
            if (item == null)
            {
                return 0;
            }

            return item.Count;
        }

        [HttpPost]
        public IActionResult AddBasket(int id, int quantity)
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
                item = new BasketItemViewModel { ItemId = id, Count = quantity };
                items.Add(item);
            }
            else
            {

            }
            {
                item.Count += quantity;
            }


            itemIdsStr = JsonConvert.SerializeObject(items);
            HttpContext.Response.Cookies.Append("basket", itemIdsStr);


            // return PartialView("_BasketPartial", _getBasket(items));
            return RedirectToAction("index");
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
                Item item1 = _context.Items.FirstOrDefault(x => x.Id == item.ItemId);
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

        [HttpPost]
        public IActionResult GetCount(int id)
        {
            return NotFound();
        }
    }
}
