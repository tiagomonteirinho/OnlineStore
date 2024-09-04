using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using UF5423_SuperShop.Helpers;
using UF5423_SuperShop.Models;

namespace UF5423_SuperShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;

        public AccountController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        public IActionResult Login() // Show login view.
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) // Access account.
        {
            if (ModelState.IsValid) // If input data is valid
            {
                var result = await _userHelper.LoginAsync(model); // Login.
                if (result.Succeeded) // If user login in successful
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl")) // If request contains "ReturnURL"
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First()); // Redirect to such URL.
                    }

                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login");
            return View(model); // Keep input data.
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
