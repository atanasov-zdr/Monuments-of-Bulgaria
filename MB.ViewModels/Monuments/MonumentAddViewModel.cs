namespace MB.ViewModels.Monuments
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using Common;

    public class MonumentAddViewModel
    {
        [Required]
        [StringLength(GlobalConstants.MaxStringLength, MinimumLength = GlobalConstants.MinStringLength)]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        public IFormFile Photo { get; set; }

        [Required]
        [Display(Name = "Oblasts")]
        public int SelectedOblastId { get; set; }
        public IEnumerable<SelectListItem> Oblasts { get; set; }
    }
}
