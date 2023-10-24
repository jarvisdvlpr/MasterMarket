using Ganss.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewMasterMarket.Areas.Manage.ViewModels;
using NewMasterMarket.Areas.Manage.ViewModels.FormViewModels;
using NewMasterMarket.DAL;
using NewMasterMarket.Models;

namespace NewMasterMarket.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            ProductViewModel productView = new ProductViewModel() { ItemList = _context.Items.Include(x => x.Category).Where(x => x.IsDeleted == false).ToList() };
            return View(productView);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");
            ViewBag.Companies = new SelectList(_context.Companies.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductFormViewModel product)
        {
            if (product.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Фотография не мужет быть пустым!");
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");
                ViewBag.Companies = new SelectList(_context.Companies.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");
                return View();
            }

            if (product.ImageFile.ContentType != "image/jpeg" && product.ImageFile.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageFile", "Файл должен быть либо .jpeg либо .png!");
                ViewBag.Categories = new SelectList(_context.Categories.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");
                ViewBag.Companies = new SelectList(_context.Companies.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");
                return View();
            }
            if (product.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "Файл должен весить не больше 2 мб!");
                ViewBag.Categories = new SelectList(_context.Categories.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");
                ViewBag.Companies = new SelectList(_context.Companies.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");
                return View();
            }
            Item itemska = new Item
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CompanyId = product.CompanyId,
                CategoryId = product.CategoryId,
                Code = Guid.NewGuid().ToString()
            };

            itemska.Imgsrc = Guid.NewGuid().ToString() + product.ImageFile.FileName;



            string path = Path.Combine(_env.WebRootPath, "uploads/items", itemska.Imgsrc);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                product.ImageFile.CopyTo(stream);
            }



            _context.Items.Add(itemska);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Item item = _context.Items.FirstOrDefault(x => x.Id == id);
            if (item == null)
                return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");
            ViewBag.Companies = new SelectList(_context.Companies.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");

            TempData["ImgLink"] = item.Imgsrc;
            TempData["ProductId"] = item.Id;

            ProductEditViewModel productEdit = new ProductEditViewModel
            {
                Name = item.Name,
                Price = item.Price,
                Description = item.Description,
                CompanyId = item.CompanyId,
                CategoryId = item.CategoryId
            };

            return View(productEdit);
        }


        [HttpPost]
        public IActionResult Edit(ProductEditViewModel productEdit)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");
                ViewBag.Companies = new SelectList(_context.Companies.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");
                return View(productEdit);
            }
            Item itemska = _context.Items.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.Id == (int)TempData["ProductId"]);
            if (itemska == null)
                return NotFound();


            if (productEdit.ImageFile == null)
            {
                itemska.Imgsrc = (string)TempData["ImgLink"];
            }
            else
            {
                if (productEdit.ImageFile.ContentType != "image/jpeg" && productEdit.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Файл должен быть либо .jpeg либо .png!");
                    ViewBag.Categories = new SelectList(_context.Categories.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");
                    ViewBag.Companies = new SelectList(_context.Companies.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");
                    return View(productEdit);
                }
                if (productEdit.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "Файл должен весить не больше 2 мб!");
                    ViewBag.Categories = new SelectList(_context.Categories.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");
                    ViewBag.Companies = new SelectList(_context.Companies.Where(x => x.IsDeleted == false).ToList(), "Id", "Name");
                    return View(productEdit);
                }




                itemska.Imgsrc = Guid.NewGuid().ToString() + productEdit.ImageFile.FileName;

                //string path = _env.WebRootPath + @"manage/uploads/items" + itemska.Imgsrc;

                string path = Path.Combine(_env.WebRootPath, "uploads/items", itemska.Imgsrc);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    productEdit.ImageFile.CopyTo(stream);
                }


                string tempData = (string)TempData["ImgLink"];
                if (!string.IsNullOrEmpty(tempData))
                {
                    string oldPath = Path.Combine(_env.WebRootPath, "uploads/items", tempData);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
            }


            itemska.Name = productEdit.Name;
            itemska.Price = productEdit.Price;
            itemska.Description = productEdit.Description;
            itemska.CategoryId = productEdit.CategoryId;
            itemska.CompanyId = productEdit.CompanyId;

            _context.Items.Update(itemska);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var item = _context.Items.Where(x=>x.IsDeleted==false).FirstOrDefault(x => x.Id == id);
            if (item == null)
                return NotFound();

            string path = Path.Combine(_env.WebRootPath, "uploads/items", item.Imgsrc);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }


            item.IsDeleted = true;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //public IActionResult ConvertExcel()
        //{
        //    string path = Path.Combine(_env.WebRootPath, "CeyhunMasterMarket.xlsx");
        //    var excels = new ExcelMapper(path).Fetch<DExcelViewModel>();
        //    List<DExcelViewModel> myview = excels.ToList();

        //    foreach (var item in myview)
        //    {
        //        Item item1 = new Item
        //        {
        //            CompanyId = 4,
        //            CategoryId = 4,
        //            Name = item.Name,
        //            Price = item.Price,
        //            Description = item.OrderName,
        //            Imgsrc = "911603e2-cc39-47e9-a45b-62cf05b87bb7photo_2023-03-04_05-02-55.jpg",
        //            Code = Guid.NewGuid().ToString()
        //        };
        //        _context.Items.Add(item1);
        //    }
        //    _context.SaveChanges();
        //    return RedirectToAction("index");
        //}


    }
}
