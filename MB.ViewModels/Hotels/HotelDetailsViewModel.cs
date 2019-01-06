namespace MB.ViewModels.Hotels
{
    using System.Collections.Generic;
    
    using ViewModels.Hotels.HotelComments;
    using ViewModels.Hotels.HotelReviews;

    public class HotelDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Stars { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public double OverallRating { get; set; }

        public int ReviewsCount { get; set; }

        public int CommentsCount { get; set; }

        public bool HasReview { get; set; }

        public HotelReviewsViewModel Reviews { get; set; }

        public IEnumerable<HotelCommentViewModel> Comments { get; set; }
    }
}
