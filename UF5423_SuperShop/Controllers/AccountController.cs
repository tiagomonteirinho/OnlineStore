using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using UF5423_SuperShop.Data;
using UF5423_SuperShop.Data.Entities;
using UF5423_SuperShop.Helpers;
using UF5423_SuperShop.Models;

namespace UF5423_SuperShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly ICountryRepository _countryRepository;

        public AccountController(IUserHelper userHelper, ICountryRepository countryRepository)
        {
            _userHelper = userHelper;
            _countryRepository = countryRepository;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) // If user is logged in
            {
                return RedirectToAction("Index", "Home"); // Show home index view.
            }

            return View(); // Show login view.
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) // Access account.
        {
            if (ModelState.IsValid) // If input data is valid
            {
                var result = await _userHelper.LoginAsync(model); // Login.
                if (result.Succeeded) // If user login in successful
                {
                    //if (this.Request.Query.Keys.Contains("ReturnUrl")) // If request contains "ReturnURL"
                    //{
                    //    return Redirect(this.Request.Query["ReturnUrl"].First()); // Redirect to such URL.
                    //}
                    // Replaced by NotAuthorized.cshtml at Startup.cs.

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

        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                Countries = _countryRepository.GetComboCountries(),
                Cities = _countryRepository.GetComboCities(0),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user == null) // If account doesn't exist
                {
                    var city = await _countryRepository.GetCityAsync(model.CityId);

                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.Username, // IdentityUser built-in required field.
                        Email = model.Username,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        City = city,
                        CityId = city.Id,
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "Could not register user account."); // Show error message at view.
                        return View(model);
                    }

                    await _userHelper.AddUserToRoleAsync(user, "Customer");
                    var isInRole = await _userHelper.IsUserInRoleAsync(user, "Customer"); // Ensure user is in role.
                    if (!isInRole) // If user is not in role
                    {
                        await _userHelper.AddUserToRoleAsync(user, "Customer"); // Force user addition to role.
                    }

                    var loginViewModel = new LoginViewModel // Create account login view model.
                    {
                        UserName = model.Username,
                        Password = model.Password,
                        RememberMe = false,
                    };

                    var result2 = await _userHelper.LoginAsync(loginViewModel); // Login automatically after registration.
                    if (result2.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, "Could not log in.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel(); // New user info.
            if (user != null) // If user exists
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Address = user.Address;
                model.PhoneNumber = user.PhoneNumber;

                var city = await _countryRepository.GetCityAsync(user.CityId);
                if (city != null)
                {
                    var country = await _countryRepository.GetCountryAsync(city);
                    if (country != null)
                    {
                        model.CountryId = country.Id;
                        model.Countries = _countryRepository.GetComboCountries();
                        model.Cities = _countryRepository.GetComboCities(country.Id);
                        model.CityId = user.CityId;
                    }
                }
            }

            model.Cities = _countryRepository.GetComboCities(model.CountryId);
            model.Countries = _countryRepository.GetComboCountries();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;

                    var city = await _countryRepository.GetCityAsync(model.CityId);
                    user.City = city;
                    user.CityId = model.CityId;


                    var response = await _userHelper.UpdateUserAsync(user);
                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated successfully";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View(); // Only show form without model data.
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }
            }
            return this.View(model);
        } 

        public IActionResult NotAuthorized()
        {
            return View();
        }

        [HttpPost] // Action type.
        [Route("Account/GetCitiesAsync")] // Action URL route.
        public async Task<JsonResult> GetCitiesAsync(int countryId) // Use Ajax to only update a part of the view and not the whole view.
        {
            var country = await _countryRepository.GetCountryWithCitiesAsync(countryId); // Get country cities list.
            return Json(country.Cities.OrderBy(c => c.Name)); // Convert list to JSON format.
            // TODO: Fix crash when option 0 ('(Select a city...)') is manually selected.
        }
    } 
}
