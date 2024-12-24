using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using OnlineStore.Data;
using OnlineStore.Data.Entities;
using OnlineStore.Helpers;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly ICountryRepository _countryRepository;
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;

        public AccountController(IUserHelper userHelper, ICountryRepository countryRepository, IConfiguration configuration, IMailHelper mailHelper)
        {
            _userHelper = userHelper;
            _countryRepository = countryRepository;
            _configuration = configuration;
            _mailHelper = mailHelper;
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
                        CityId = model.CityId,
                        City = city,
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

                    //var loginViewModel = new LoginViewModel // Create account login view model.
                    //{
                    //    UserName = model.Username,
                    //    Password = model.Password,
                    //    RememberMe = false,
                    //};

                    //var result2 = await _userHelper.LoginAsync(loginViewModel); // Login automatically after registration.

                    string emailToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user); // Generate email address confirmation token.
                    string tokenLink = Url.Action( // Execute action when token link is clicked.
                        "ConfirmEmail", // Action name.
                        "Account", // Action controller.
                        new // Action parameters
                        {
                            userId = user.Id,
                            token = emailToken,
                        },
                        protocol: HttpContext.Request.Scheme
                    );

                    Response response = _mailHelper.SendEmail(model.Username, "SuperShop account confirmation", $"<h1>SuperShop account confirmation</h1>" + $"To confirm your account, please open the following link: </br><a href= \"{tokenLink}\">Confirm account</a>");
                    if (response.IsSuccessful)
                    {
                        ViewBag.Message = "An account confirmation email has been sent to your email address. Please follow the instructions to verify your email address.";
                        return View(model);
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

        [HttpPost]
        public async Task<IActionResult> CreateApiToken([FromBody] LoginViewModel model) // Create API token which expires after a defined time period.
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.UserName);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        var claims = new[] // Create token claims.
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email), // Register user email.
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Create random guid for registered email.
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])); // Get tokens key from 'appsettings.json' and encrypt it by converting it to UTF8 bytes. // 'SymmetricSecurityKey': encryption method.
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // 'HmacSha256': 256 bit SSL security algorithm.
                        var token = new JwtSecurityToken // Generate API token.
                        (
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15), // Token duration period until expiration.
                            signingCredentials: credentials
                        );
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                        };

                        return this.Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token)) // If parameters are empty or missing
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null) // If user doesn't exist
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) // If token and user don't match
            {
                return NotFound();
            }

            return View();
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "That email address is not registered to any account.");
                    return View(model);
                }

                var passwordToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                var tokenLink = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = passwordToken },
                    protocol: HttpContext.Request.Scheme
                );

                Response response = _mailHelper.SendEmail(model.Email, "SuperShop password recovery", $"<h1>SuperShop password recovery</h1>" + $"To reset your password, please open the following link: </br><a href= \"{tokenLink}\">Reset password</a>");

                if (response.IsSuccessful)
                {
                    ViewBag.Message = "A password recovery email has been sent to your email address. Please follow the instructions to reset your password.";
                }

                return View();
            }

            return View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Username);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.NewPassword);
                if(!result.Succeeded) // If token doesn't exist or has expired
                {
                    ViewBag.Message = "Unable to reset password.";
                    return View(model);
                }

                ViewBag.Message = "Password successfully changed.";
                return View();
            }

            ViewBag.Message = "User not found.";
            return View(model);
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
