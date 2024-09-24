using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UF5423_SuperShop.Data.Entities
{
    public class User : IdentityUser // 'IdentityUser': ASP.NET Core built-in user class.
    {
        [MaxLength(99, ErrorMessage = "The field {0} cannot exceed {1} characters.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; } // Custom user property.


        [MaxLength(99, ErrorMessage = "The field {0} cannot exceed {1} characters.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [MaxLength(99, ErrorMessage = "The field {0} cannot exceed {1} characters.")]
        public string Address { get; set; }

        public City City { get; set; }

        public int CityId { get; set; } // Set property object ID to show at database because 'IdentityUser' property objects don't.

        [Display(Name = "Full name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
