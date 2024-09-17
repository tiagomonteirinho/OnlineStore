using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UF5423_SuperShop.Data.Entities
{
    public class User : IdentityUser // 'IdentityUser': ASP.NET Core built-in user class.
    {

        [Display(Name = "First name")]
        public string FirstName { get; set; } // Custom user property.


        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Full name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
