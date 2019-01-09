namespace MB.ViewModels.Monuments
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using Common;

    public class MonumentEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.MaxStringLength, MinimumLength = GlobalConstants.MinStringLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile Photo { get; set; }

        [Required]
        [Display(Name = "Oblasts")]
        public int SelectedOblastId { get; set; }
        public IEnumerable<SelectListItem> Oblasts { get; set; }
    }
}
