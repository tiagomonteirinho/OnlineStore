using System;
using System.ComponentModel.DataAnnotations;

namespace UF5423_SuperShop.Data.Entities
{
    public class Product
    {
        //public int Id { get; set; } // Automatically set as primary key by having default property name 'Id' of type int.

        [Display(Name = "Id")]
        [Key] // Set as primary key with custom name or type.
        public int ProductId { get; set; }

        [Required] // Set as required value.
        [MaxLength(99, ErrorMessage = "The field {0} cannot exceed {1} characters.")] // Set maximum input length and error message.
        [Display(Name = "Name")]
        public string ProductName { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] // 'DataFormatString': default format. 'ApplyFormatInEditMode': force format in edit mode. 'C': currency.
        public decimal ProductPrice { get; set; }

        [Display(Name = "Image")] // Manually set UI name.
        public string ProductImageUrl { get; set; }

        [Display(Name = "Last Purchase")]
        public DateTime? ProductLastPurchase {  get; set; }

        [Display(Name = "Last Sale")]
        public DateTime? ProductLastSale { get; set; }

        [Display(Name = "Is Available")]
        public bool ProductIsAvailable { get; set; }

        [Display(Name = "Stock")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double ProductStock { get; set; }
    }
}
