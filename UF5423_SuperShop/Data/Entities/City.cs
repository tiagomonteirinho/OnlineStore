using System.ComponentModel.DataAnnotations;

namespace UF5423_SuperShop.Data.Entities
{
    public class City : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "City")]
        [MaxLength(99, ErrorMessage = "The field {0} cannot exceed {1} characters.")]
        public string Name { get; set; }
    }
}
