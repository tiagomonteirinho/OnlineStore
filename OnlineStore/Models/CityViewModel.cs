using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class CityViewModel
    {
        public int CountryId { get; set; }

        public int CityId { get; set; }

        [Required]
        [Display(Name = "City")]
        [MaxLength(99, ErrorMessage = "The field {0} cannot exceed {1} characters.")]
        public string Name { get; set; }

    }
}
