using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UF5423_SuperShop.Data.Entities
{
    public class Country : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(99, ErrorMessage = "The field {0} cannot exceed {1} characters.")]
        public string Name { get; set; }

        public ICollection<City> Cities { get; set; }

        [Display(Name = "Number of cities")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;
    }
}
