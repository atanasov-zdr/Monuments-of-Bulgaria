namespace MB.ViewModels.Hotels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using Common;

    public class HotelEditViewModel
    {
        private const int MinStars = 1;
        private const int MaxStars = 5;

        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.MaxStringLength, MinimumLength = GlobalConstants.MinStringLength)]
        public string Name { get; set; }

        [Required]
        [Range(MinStars, MaxStars)]
        public int Stars { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(GlobalConstants.MaxStringLength, MinimumLength = GlobalConstants.MinStringLength)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        [Phone]
        [StringLength(GlobalConstants.MaxPhoneNumberLength, MinimumLength = GlobalConstants.MinPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        public IFormFile Photo { get; set; }

        [Required]
        [Display(Name = "Oblasts")]
        public int SelectedOblastId { get; set; }
        public IEnumerable<SelectListItem> Oblasts { get; set; }
    }
}
