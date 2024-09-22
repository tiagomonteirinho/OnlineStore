﻿using System.ComponentModel.DataAnnotations;
using System;

namespace UF5423_SuperShop.Models
{
    public class DeliveryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Delivery date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DeliveryDate { get; set; } // Removed '?' from value type.
    }
}
