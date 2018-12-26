﻿namespace MB.ViewModels.Hotels
{
    using System.Collections.Generic;
    
    using ViewModels.Hotels.HotelComments;
    using ViewModels.Hotels.HotelReviews;

    public class HotelDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public double OverallRating { get; set; }

        public int ReviewsCount { get; set; }

        public int CommentsCount { get; set; }

        public HotelReviewsViewModel Reviews { get; set; }

        public IEnumerable<HotelCommentViewModel> Comments { get; set; }
    }
}