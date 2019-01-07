namespace MB.ViewModels.Hotels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class HotelEditViewModel
    {
        private const int MinLength = 3;
        private const int MaxLength = 100;
        private const int MinStars = 1;
        private const int MaxStars = 5;
        private const int MinPhoneNumberLength = 6;
        private const int MaxPhoneNumberLength = 10;

        public int Id { get; set; }

        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
        public string Name { get; set; }

        [Required]
        [Range(MinStars, MaxStars)]
        public int Stars { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        [Phone]
        [StringLength(MaxPhoneNumberLength, MinimumLength = MinPhoneNumberLength)]
        public string PhoneNumber { get; set; }
        
        [Required]
        [Display(Name = "Oblasts")]
        public int SelectedOblastId { get; set; }
        public IEnumerable<SelectListItem> Oblasts { get; set; }
    }
}
