using NewMasterMarket.Models;

namespace NewMasterMarket.ViewModels
{
    public class JustCategoryViewModel
    {
        public List<Category> CategoryList { get; set; }
        public Category CategoryItem { get; set; }
        public List<Item> ItemList { get; set; }
    }
}
