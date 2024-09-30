using System.ComponentModel.DataAnnotations;

namespace UF5423_SuperShop.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
