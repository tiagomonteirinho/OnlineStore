using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using static System.Net.Mime.MediaTypeNames;

namespace UF5423_SuperShop.Data.Entities
{
    public class Product : IEntity
    {
        [Display(Name = "Id")]
        public int Id { get; set; } // Automatically set as primary key by having default property name 'Id' of type int.

        //[Display(Name = "Id")]
        //[Key] // Set as primary key with custom property name or type.
        //public int ProductId { get; set; }

        [Required] // Set as required value.
        [MaxLength(99, ErrorMessage = "The field {0} cannot exceed {1} characters.")] // Set maximum input length and error message.
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)] // 'DataFormatString': default format. 'ApplyFormatInEditMode': force format in edit mode. 'C': currency.
        public decimal Price { get; set; }

        [Display(Name = "Image")] // Manually set UI name.
        public string ImageUrl { get; set; }

        [Display(Name = "Last Purchase")]
        public DateTime? LastPurchase { get; set; }

        [Display(Name = "Last Sale")]
        public DateTime? LastSale { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Stock")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }

        public User User { get; set; }

        public string ImageFullPath
        {
            //TODO: Add blobs.
            get
            {
                if (string.IsNullOrEmpty(ImageUrl))
                {
                    //return $"https://tiagomonteirinho.somee.com/images/image_unavailable.jpg";
                    return $"https://localhost:44333//images/image_unavailable.jpg";
                }

                //return $"https://tiagomonteirinho.somee.com/{ImageUrl.Substring(1)}"; // 'Substring(1)': remove first string character ('~').
                return $"https://localhost:44333/{ImageUrl.Substring(1)}";
            }
        } 
    }
}
