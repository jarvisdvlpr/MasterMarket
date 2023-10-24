using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewMasterMarket.Areas.Manage.ViewModels;
using NewMasterMarket.DAL;

namespace NewMasterMarket.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        #region AdminCreated
        //public async Task<IActionResult> Test()
        //{
        //    var result1 = await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    var result2 = await _roleManager.CreateAsync(new IdentityRole("Member"));
        //    var result3 = await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

        //    IdentityUser admin = await _userManager.FindByNameAsync("SuperAdmin");
        //    var result = await _userManager.AddToRoleAsync(admin, "SuperAdmin");

        //    return Ok();

        //}

        //public async Task<IActionResult> test()
        //{
        //    IdentityUser appUser = new IdentityUser
        //    {
        //        UserName = "SuperAdmin",
        //        Email = "superadmin@gmail.com"
        //    };

        //    var result = await _userManager.CreateAsync(appUser, "Admin123");
        //    return View();
        //}
        #endregion
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            IdentityUser appUser = await _userManager.FindByNameAsync(loginVM.UserName);
            if (appUser == null)
            {
                ModelState.AddModelError("Password", "Username or Password is incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.IsPersistent, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Password", "Username or Password is incorrect");
                return View();
            }
            return RedirectToAction("Index", "Dashboard");
        }





        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("login", "account");
        }
    }
}
