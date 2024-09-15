using Microsoft.AspNetCore.Identity;

namespace UF5423_SuperShop.Data.Entities
{
    public class User : IdentityUser // 'IdentityUser': ASP.NET Core built-in user class.
    {
        public string FirstName { get; set; } // Custom user property.

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
