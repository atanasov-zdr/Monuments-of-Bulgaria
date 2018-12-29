namespace MB.Models.Monuments
{
    using System.Collections.Generic;
    using System.Linq;

    using Base;
    using Oblasts;
    using Trips;

    public class Monument : BaseModel<int>
    {
        public Monument()
        {
            this.MonumentReviews = new HashSet<MonumentReview>();
            this.MonumentComments = new HashSet<MonumentComment>();
            this.Trips = new HashSet<Trip>();
        }

        public string Name { get; set; }

        public string  Description { get; set; }

        public string ImageUrl { get; set; }

        public double OverallRating => this.MonumentReviews.Any() ? this.MonumentReviews.Average(x => (int)x.Rating) : 0;

        public int ReviewsCount => this.MonumentReviews.Count();

        public int CommentsCount => this.MonumentComments.Count();

        public int OblastId { get; set; }
        public virtual Oblast Oblast { get; set; }

        public virtual IEnumerable<MonumentReview> MonumentReviews { get; set; }

        public virtual IEnumerable<MonumentComment> MonumentComments { get; set; }

        public virtual IEnumerable<Trip> Trips { get; set; }
    }
}
