using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [MaxLength(99, ErrorMessage = "The field {0} cannot exceed {1} characters.")]
        public string Address { get; set; }

        [MaxLength(20, ErrorMessage = "The field {0} cannot exceed {1} characters.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Country")]
        [Range(1, int.MaxValue, ErrorMessage = "A country must be selected")]
        public int CountryId { get; set; }

        [Display(Name = "City")]
        [Range(1, int.MaxValue, ErrorMessage = "A city must be selected")]
        public int CityId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
