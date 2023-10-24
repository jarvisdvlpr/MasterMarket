using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewMasterMarket.Areas.Manage.ViewModels;
using NewMasterMarket.DAL;
using NewMasterMarket.Models;

namespace NewMasterMarket.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class ClientController : Controller
    {
        private readonly AppDbContext _context;

        public ClientController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            BackClientViewModel backClientView = new BackClientViewModel()
            {
                Clients = _context.Clients.Where(x => x.IsDeleted == false).ToList()
            };
            return View(backClientView);
        }

        public IActionResult Edit(int id)
        {
            Client client = _context.Clients.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.Id == id);
            if (client == null) return NotFound();

            return View(client);
        }

        [HttpPost]
        public IActionResult Edit(Client client)
        {
            if (!ModelState.IsValid) 
                return View();

            _context.Update(client);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            Client client = _context.Clients.Where(x => x.IsDeleted == false).FirstOrDefault(x => x.Id == id);
            if (client == null)
                return NotFound();

            client.IsDeleted = true;
            _context.Update(client);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
