using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using UF5423_SuperShop.Data.Entities;

namespace UF5423_SuperShop.Models
{
    public class ProductViewModel : Product // View model with values not defined in inherited entity.
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; } // Property that receives files that are not stored in database.
    }
}
