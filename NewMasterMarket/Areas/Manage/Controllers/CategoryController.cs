using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewMasterMarket.Areas.Manage.ViewModels;
using NewMasterMarket.Areas.Manage.ViewModels.FormViewModels;
using NewMasterMarket.DAL;
using NewMasterMarket.Models;

namespace NewMasterMarket.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            CategoryViewModel categoryView = new CategoryViewModel() { CategoryList = _context.Categories.Where(x => !x.IsDeleted).ToList() };
            return View(categoryView);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryFormViewModel categoryForm)
        {
            if (categoryForm.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Иконка не мужет быть пустым!");

            if (!ModelState.IsValid)
                return View();




            if (categoryForm.ImageFile.ContentType != "image/svg+xml")
            {
                ModelState.AddModelError("ImageFile", "Иконка может быть ТОЛЬКО в формате svg");
                return View();
            }
            if (categoryForm.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "Иконка не может весить больше 2mb");
                return View();
            }
            Category category = new Category();



            category.IconSrc = Guid.NewGuid().ToString() + categoryForm.ImageFile.FileName;
            category.Name = categoryForm.Name;



            string path = Path.Combine(_env.WebRootPath, "uploads/category", category.IconSrc);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                categoryForm.ImageFile.CopyTo(stream);
            }

            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null) return NotFound();
            TempData["catImg"] = category.IconSrc;
            CategoryEditFormViewModel categoryEdit = new CategoryEditFormViewModel { Name = category.Name };
            return View(categoryEdit);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category.Name.Length >= 1000)
            {
                ModelState.AddModelError("Name", "Называние категории не может быть выше 1000и символов!");
                return View(category);
            }
            if (string.IsNullOrEmpty(category.Name))
            {
                ModelState.AddModelError("Name", "Называние категории не может быть пустым!");
                return View(category);
            }
            if (category.ImageFile == null)
            {
                category.IconSrc = (string)TempData["catImg"];
            }
            else
            {
                if (category.ImageFile.ContentType != "image/svg+xml")
                {
                    ModelState.AddModelError("ImageFile", "Иконка может быть ТОЛЬКО в формате svg");
                    return View();
                }
                if (category.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "Иконка не может весить больше 2mb");
                    return View(category);
                }
                category.IconSrc = Guid.NewGuid().ToString() + category.ImageFile.FileName;
                string path = Path.Combine(_env.WebRootPath, "uploads/category", category.IconSrc);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    category.ImageFile.CopyTo(stream);
                }

                string tempData = (string)TempData["catImg"];
                if (!string.IsNullOrEmpty(tempData))
                {
                    string oldPath = Path.Combine(_env.WebRootPath, "uploads/category", tempData);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
            }
            _context.Categories.Update(category);
            _context.SaveChanges();
            return RedirectToAction("index");
        }


        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
                return NotFound();

            string path = Path.Combine(_env.WebRootPath, "uploads/category", category.IconSrc);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }


            category.IsDeleted = true;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
