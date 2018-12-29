namespace MB.ViewModels.Monuments.MonumentReviews
{
    using System.ComponentModel.DataAnnotations;

    using Models.Enums;

    public class MonumentReviewWriteViewModel
    {
        public int MonumentId { get; set; }

        public string MonumentName { get; set; }

        [Required]
        public Rating? Rating { get; set; }

        [Required]
        [Display(Name = "Time of Year")]
        public Season? TimeOfYear { get; set; }

        [Required]
        [Display(Name = "Traveller Type")]
        public TravellerType? TravellerType { get; set; }
    }
}
