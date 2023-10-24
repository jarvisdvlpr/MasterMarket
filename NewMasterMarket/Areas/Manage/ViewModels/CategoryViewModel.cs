using NewMasterMarket.Models;

namespace NewMasterMarket.Areas.Manage.ViewModels
{
    public class CategoryViewModel
    {
        public List<Category> CategoryList { get; set; }
        public Category CategoryItem { get; set; }
        public List<Item> ItemList { get; set; }
    }
}
