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
    public class CompanyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CompanyController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            CompanyViewModel companyViewModel = new CompanyViewModel() { CompanyList = _context.Companies.Where(x => x.IsDeleted == false).ToList() };

            return View(companyViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CompanyFormViewModel companyForm)
        {
            if (companyForm.ImageFile == null)
                ModelState.AddModelError("ImageFile", "Иконка не мужет быть пустым!");
            if (!ModelState.IsValid)
                return View();

            if (companyForm.ImageFile.ContentType != "image/jpeg" && companyForm.ImageFile.ContentType != "image/png" && companyForm.ImageFile.ContentType != "image/svg+xml")
            {
                ModelState.AddModelError("ImageFile", "Файл должен быть либо .jpeg либо .png!");
                return View();
            }
            if (companyForm.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "Иконка не может весить больше 2mb");
                return View();
            }
            Company company = new Company();

            company.ImgSrc = Guid.NewGuid().ToString() + companyForm.ImageFile.FileName;
            company.Name = companyForm.Name;
            company.Description = companyForm.Description;

            string path = Path.Combine(_env.WebRootPath, "uploads/company", company.ImgSrc);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                companyForm.ImageFile.CopyTo(stream);
            }

            _context.Companies.Add(company);
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Company company = _context.Companies.FirstOrDefault(x => x.Id == id);
            if (company == null) return NotFound();
            TempData["categoryImg"] = company.ImgSrc;
            TempData["companyId"] = id;
            CompanyEditFormViewModel companyEditFormViewModel = new CompanyEditFormViewModel() { 
                Name = company.Name,
                Description = company.Description
            };
            return View(companyEditFormViewModel);
        }

        [HttpPost]
        public IActionResult Edit(CompanyEditFormViewModel companyEdit)
        {
            if (companyEdit.Name.Length >= 1000)
            {
                ModelState.AddModelError("Name", "Называние категории не может быть выше 1000и символов!");
                return View(companyEdit);
            }
            if (string.IsNullOrEmpty(companyEdit.Name))
            {
                ModelState.AddModelError("Name", "Называние категории не может быть пустым!");
                return View(companyEdit);
            }

            //Company company = new Company() { Name = companyEdit.Name, Description = companyEdit.Description};
            Company company = _context.Companies.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.Id == (int)TempData["companyId"]);
            if (company == null)
            {
                return NotFound();
            }

            company.Name = companyEdit.Name;
            company.Description = companyEdit.Description;
            

            if (companyEdit.ImageFile == null)
            {
                company.ImgSrc = (string)TempData["categoryImg"];
            }
            else
            {
                if (companyEdit.ImageFile.ContentType != "image/jpeg" && companyEdit.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Файл должен быть либо .jpeg либо .png!");
                    return View(companyEdit);
                }
                if (companyEdit.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "Иконка не может весить больше 2mb");
                    return View(companyEdit);
                }
                company.ImgSrc = Guid.NewGuid().ToString() + companyEdit.ImageFile.FileName;
                string path = Path.Combine(_env.WebRootPath, "uploads/company", company.ImgSrc);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    companyEdit.ImageFile.CopyTo(stream);
                }

                string tempData = (string)TempData["categoryImg"];
                if (!string.IsNullOrEmpty(tempData))
                {
                    string oldPath = Path.Combine(_env.WebRootPath, "uploads/company", tempData);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
            }
            _context.Companies.Update(company);
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Company company = _context.Companies.FirstOrDefault(x => x.Id == id);
            if (company == null)
                return NotFound();

            string path = Path.Combine(_env.WebRootPath, "uploads/company", company.ImgSrc);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }


            company.IsDeleted = true;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
