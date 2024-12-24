using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
