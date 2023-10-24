using Microsoft.AspNetCore.Identity;
using NewMasterMarket.DAL;
using NewMasterMarket.Models;
using NewMasterMarket.ViewModels;
using Newtonsoft.Json;


namespace NewMasterMarket.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
       
        

        public LayoutService(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            
            
        }

        //public async Task<string> GetUserRole(string name)
        //{
        //    var user = await _userManager.FindByNameAsync(name);
        //    var roles = await _userManager.GetRolesAsync(user);

        //   return roles.ToString();
        //}

        public BasketViewModel GetBasket()
        {
            BasketViewModel BasketVM = new BasketViewModel
            {
                BasketItems = new List<ProductBasketItemViewModel>(),
                TotalPrice = 0
            };

            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
            var basketStr = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
            if (!string.IsNullOrWhiteSpace(basketStr))
                basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketStr);

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

        public bool CookiesShow()
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies["koki"] == null)
            {
                return false;
            }
            if (_httpContextAccessor.HttpContext.Request.Cookies["koki"] == "true")
            {
                return true;
            }
            return false;
        }

        public bool IsWrited()
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies["name"] == null)
            {
                return false; 
            }
            else
            {
                return true;
            }
        }

        public string DeliveryName()
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies["name"] == null)
            {
                return null;
            }
            return _httpContextAccessor.HttpContext.Request.Cookies["name"].ToString();
        }

        public Client GetClient()
        {
            string cookieName = _httpContextAccessor.HttpContext.Request.Cookies["name"];
            if (cookieName == null)
            {
                return null;
            }
            Client client = _context.Clients.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.Fullname == cookieName);
            if (client == null)
            {
                return null;
            }
            return client;
        }
        public List<Category> GetCategories()
        {
            return _context.Categories.Where(x => x.IsDeleted == false).ToList();
        }
    }
}
