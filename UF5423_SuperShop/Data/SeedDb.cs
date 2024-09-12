using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UF5423_SuperShop.Data.Entities;
using UF5423_SuperShop.Helpers;

namespace UF5423_SuperShop.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        //private readonly UserManager<User> _userManager; // Seed user (administrator). // Replaced by user helper.
        private Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper) // Define custom data context and user helper classes.
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); // Create database if it doesn't exist.
            await _userHelper.CheckRoleAsync("Admin"); // Create user role if it doesn't exist.
            await _userHelper.CheckRoleAsync("Customer");

            var user = await _userHelper.GetUserByEmailAsync("tiagomonteirinho.spam@gmail.com"); // Define seed user if exists.
            if (user == null) // If user doesn't exist
            {
                user = new User // Define user entity data.
                {
                    FirstName = "Tiago",
                    LastName = "Monteirinho",
                    Email = "tiagomonteirinho.spam@gmail.com",
                    UserName = "tiagomonteirinho.spam@gmail.com",
                    PhoneNumber = "123456789"
                };

                var result = await _userHelper.AddUserAsync(user, "123456"); // Create user with entity data and password.
                if (result != IdentityResult.Success) // If user still doesn't exist
                {
                    throw new InvalidOperationException("Unable to create seed user.");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin"); // Ensure user is in role.
            if (!isInRole) // If user is not in role
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin"); // Force user addition to role.
            }

            if (!_context.Products.Any()) // If data base is empty
            {
                AddProduct("Galaxy A53", user);
                AddProduct("Galaxy Tab A6", user);
                AddProduct("Galaxy Watch 7", user);
                AddProduct("Galaxy Buds 3", user);
                AddProduct("Galaxy Book 4", user);

                await _context.SaveChangesAsync(); // Save default data to data base.
            }
        }

        private void AddProduct(string productName, User user) // Define default product data and user.
        {
            _context.Products.Add(new Product
            {
                Name = productName,
                Price = _random.Next(99, 1499),
                IsAvailable = true,
                Stock = _random.Next(99, 999),
                User = user
            });
        }
    }
}