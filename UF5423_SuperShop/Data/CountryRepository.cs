using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UF5423_SuperShop.Data.Entities;
using UF5423_SuperShop.Models;

namespace UF5423_SuperShop.Data
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetCountriesWithCities() // List all countries with all their cities.
        {
            return _context.Countries
                .Include(co => co.Cities)
                .OrderBy(co => co.Name);
        }

        public async Task<Country> GetCountryWithCitiesAsync(int id) // Get country with all its cities.
        {
            return await _context.Countries
                .Include(co => co.Cities)
                .Where(co => co.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Country> GetCountryAsync(City city) // Get city country.
        {
            return await _context.Countries
                .Where(co => co.Cities.Any(ci => ci.Id == city.Id))
                .FirstOrDefaultAsync();
        }

        public async Task<City> GetCityAsync(int id) // Get city.
        {
            return await _context.Cities.FindAsync(id); // Bypass.
        }

        public async Task AddCityAsync(CityViewModel model)
        {
            var country = await this.GetCountryWithCitiesAsync(model.CountryId);
            if (country == null)
            {
                return;
            }

            country.Cities.Add(new City { Name = model.Name });
            _context.Countries.Update(country);
            await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateCityAsync(City city)
        {
            var country = await _context.Countries
                .Where(co => co.Cities.Any(ci => ci.Id == city.Id))
                .FirstOrDefaultAsync();

            _context.Cities.Update(city);
            await _context.SaveChangesAsync();
            return country.Id; // Keep country.
        }

        public async Task<int> DeleteCityAsync(City city)
        {
            var country = await _context.Countries
                .Where(co => co.Cities.Any(ci => ci.Id == city.Id))
                .FirstOrDefaultAsync();

            if (country == null)
            {
                return 0;
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return country.Id;
        }

        public IEnumerable<SelectListItem> GetComboCountries()
        {
            var list = _context.Countries.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            }).OrderBy(li => li.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a country...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboCities(int countryId) // Synchronous list because it depends on another list.
        {
            var country = _context.Countries.Find(countryId);
            var list = new List<SelectListItem>();
            if (country != null)
            {
                list = country.Cities.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                }).OrderBy(li => li.Text).ToList();

                list.Insert(0, new SelectListItem
                {
                    Text = "(Select a city...)",
                    Value = "0"
                });
            }

            return list;
        }

        //public async Task<IEnumerable<SelectListItem>> GetComboRolesAsync() // Example of an asynchronous list when it is the only list.
        //{
        //    var roles = await _roleManager.Roles.ToListAsync();
        //    var list = roles.Select(r => new SelectListItem
        //    {
        //        Text = r.Name,
        //        Value = r.Name,
        //    }).OrderBy(i => i.Text).ToList();

        //    list.Insert(0, new SelectListItem
        //    {
        //        Text = "(Select a role...)",
        //        Value = "0"
        //    });

        //    return list;
        //}
    }
}
