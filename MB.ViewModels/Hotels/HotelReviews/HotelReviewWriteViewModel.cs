namespace MB.ViewModels.Hotels.HotelReviews
{
    using System.ComponentModel.DataAnnotations;

    using Models.Enums;

    public class HotelReviewWriteViewModel
    {
        public int HotelId { get; set; }

        public string HotelName { get; set; }

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
