namespace MB.ViewModels.Monuments
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class MonumentAddViewModel
    {
        private const int MinLength = 3;
        private const int MaxLength = 100;

        [Required]
        [StringLength(MaxLength, MinimumLength = MinLength)]
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
