using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Data.Entities;
using OnlineStore.Models;

namespace OnlineStore.Data
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        IQueryable GetCountriesWithCities();

        Task<Country> GetCountryWithCitiesAsync(int id);

        Task<City> GetCityAsync(int id);

        Task AddCityAsync(CityViewModel model);

        Task<int> UpdateCityAsync(City city);

        Task<int> DeleteCityAsync(City city);

        IEnumerable<SelectListItem> GetComboCountries();

        IEnumerable<SelectListItem> GetComboCities(int countryId); // Combo box list of all cities of a country.

        Task<Country> GetCountryAsync(City city);
    }
}
