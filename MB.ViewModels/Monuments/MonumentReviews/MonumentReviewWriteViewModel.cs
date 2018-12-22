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
        public Season? TimeOfYear { get; set; }

        [Required]
        public TravellerType? TravellerType { get; set; }
    }
}
