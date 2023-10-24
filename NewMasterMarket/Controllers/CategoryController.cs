using Microsoft.AspNetCore.Mvc;
using NewMasterMarket.Areas.Manage.ViewModels;
using NewMasterMarket.DAL;
using NewMasterMarket.ViewModels;

namespace NewMasterMarket.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            JustCategoryViewModel categoryView = new JustCategoryViewModel
            {
                CategoryItem = _context.Categories.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.Id == id),
                ItemList = _context.Items.Where(x => x.IsDeleted == false).Where(x => x.CategoryId == id).Take(50).ToList(),
                CategoryList = _context.Categories.Where(x => x.IsDeleted == false).ToList()
            };
            if (categoryView.CategoryItem == null)
            {
                return NotFound();
            }
            

            return View(categoryView);
        }
    }
}
