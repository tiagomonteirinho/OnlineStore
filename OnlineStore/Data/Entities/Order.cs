using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OnlineStore.Data.Entities
{
    public class Order : IEntity
    {
        public int Id { get; set; }

        [Required] // Adding '[Required]' to value type requires database reset.
        [Display(Name = "Order date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Delivery date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? DeliveryDate { get; set; } // Adding '?' to value type does not require database reset.

        [Required]
        public User User { get; set; }

        public IEnumerable<OrderDetail> Items { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")] // No decimal places.
        public int Lines => Items == null ? 0 : Items.Count();

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity => Items == null ? 0 : Items.Sum(i => i.Quantity);

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value => Items == null ? 0 : Items.Sum(i => i.Value);

        [Display(Name = "Order date")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = false)] //'tt': AM/PM.
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]

        public DateTime? OrderDateLocal => this.OrderDate == null ? null : this.OrderDate.ToLocalTime(); // Convert time to local hour format.

    }
}
