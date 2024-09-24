using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            //await _context.Database.EnsureCreatedAsync(); // Create database if it doesn't exist. // Does not create database with migrations.
            await _context.Database.MigrateAsync(); // Create database with both seed and migrations.
            await _userHelper.CheckRoleAsync("Admin"); // Create user role if it doesn't exist.
            await _userHelper.CheckRoleAsync("Customer");

            if (!_context.Countries.Any()) // If no country exists
            {
                //_context.Countries.Add(new Country
                //{
                //    Name = "Portugal",
                //    Cities = new List<City>
                //    {
                //        new City { Name = "Lisbon" },
                //        new City { Name = "Porto"},
                //        new City { Name = "Faro"},
                //        new City { Name = "Coimbra"},
                //    },
                //});

                var cities = new List<City> // Create cities.
                {
                    new City { Name = "Lisbon" },
                    new City { Name = "Porto"},
                    new City { Name = "Faro"},
                    new City { Name = "Coimbra"},
                };

                _context.Countries.Add(new Country // Create country.
                {
                    Name = "Portugal",
                    Cities = cities, // Add cities to country.
                });

                await _context.SaveChangesAsync();
            }

            var user = await _userHelper.GetUserByEmailAsync("admin@supershop.com"); // Define seed user if exists.
            if (user == null) // If user doesn't exist
            {
                user = new User // Define user entity data.
                {
                    FirstName = "Tiago",
                    LastName = "Monteirinho",
                    Email = "admin@supershop.com",
                    UserName = "admin@supershop.com",
                    PhoneNumber = "123456789",
                    Address = "Rua das Flores",
                    City = _context.Countries.FirstOrDefault().Cities.FirstOrDefault(), // First city found of first country found.
                    CityId = _context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                };

                var result = await _userHelper.AddUserAsync(user, "123456"); // Create user with entity data and password.
                if (result != IdentityResult.Success) // If user still doesn't exist
                {
                    throw new InvalidOperationException("Could not create seed user account.");
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