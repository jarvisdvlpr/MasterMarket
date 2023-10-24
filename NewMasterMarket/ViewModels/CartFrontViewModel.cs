using NewMasterMarket.Models;

namespace NewMasterMarket.ViewModels
{
    public class CartFrontViewModel
    {
        public List<Category> CategoryList { get; set; }
        public List<CartItemCountViewModel> Items { get; set; }

    }
}
