using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UF5423_SuperShop.Models
{
    public class AddItemViewModel
    {
        [Display(Name = "Product")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a product!")] // Only allow item addition if ID is 1 or higher.
        public int ProductId { get; set; }

        [Display(Name = "Product")]
        [Range(0.001, double.MaxValue, ErrorMessage = "Quantity must be positive!")]
        public int Quantity { get; set; }

        public IEnumerable<SelectListItem> Products { get; set; } // Combo box with list of products.
    }
}
