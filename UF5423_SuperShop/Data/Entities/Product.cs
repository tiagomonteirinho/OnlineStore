using System;
using System.ComponentModel.DataAnnotations;

namespace UF5423_SuperShop.Data.Entities
{
    public class Product
    {
        //public int Id { get; set; } // automatically set as primary key by having property name 'Id' of type int.

        [Display(Name = "Id")]
        [Key] // manually set as primary key with custom name or type.
        public int ProductId { get; set; }

        [Display(Name = "Name")]
        public string ProductName { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] // 'DataFormatString': default format. 'ApplyFormatInEditMode': force format in edit mode. 'C': currency.
        public decimal ProductPrice { get; set; }

        [Display(Name = "Image")] // manually set name in view.
        public string ProductImageUrl { get; set; }

        [Display(Name = "Last Purchase")]
        public DateTime ProductLastPurchase {  get; set; }

        [Display(Name = "Last Sale")]
        public DateTime ProductLastSale { get; set; }

        [Display(Name = "Is Available")]
        public bool ProductIsAvailable { get; set; }

        [Display(Name = "Stock")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)] // 'N': number (quantity).
        public double ProductStock { get; set; }
    }
}
