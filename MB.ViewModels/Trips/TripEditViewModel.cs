﻿namespace MB.ViewModels.Trips
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class TripEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        
        [Required]
        [Display(Name = "Monuments")]
        public int SelectedMonumentId { get; set; }
        public IEnumerable<SelectListItem> Monuments { get; set; }
        
        [Required]
        [Display(Name = "Hotels")]
        public int SelectedHotelId { get; set; }
        public IEnumerable<SelectListItem> Hotels { get; set; }
    }
}