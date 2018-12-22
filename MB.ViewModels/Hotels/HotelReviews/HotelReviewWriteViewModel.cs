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
    }
}
