namespace MB.ViewModels.Monuments
{
    using System.Collections.Generic;
    
    using ViewModels.Monuments.MonumentComments;
    using ViewModels.Monuments.MonumentReviews;

    public class MonumentDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public double OverallRating { get; set; }

        public int ReviewsCount { get; set; }

        public int CommentsCount { get; set; }

        public bool HasReview { get; set; }

        public MonumentReviewsViewModel Reviews { get; set; }

        public IEnumerable<MonumentCommentViewModel> Comments { get; set; }
    }
}
