using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewMasterMarket.Areas.Manage.ViewModels;
using NewMasterMarket.DAL;

namespace NewMasterMarket.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            OrderBackViewModel orderBack = new OrderBackViewModel()
            {
                Orders = _context.Orders.Where(x => x.IsDeleted == false).Include(x=>x.Items).Include(x=>x.Client).ToList()
            };
            return View(orderBack);
        }
    }
}
