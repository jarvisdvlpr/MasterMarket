using NewMasterMarket.Models;

namespace NewMasterMarket.ViewModels
{
    public class FrontProductViewModel
    {
        public Item myItem { get; set; }
        public List<Category> CategoryList { get; set; }
        public int ItemBasketCount { get; set; }
    }
}
