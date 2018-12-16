namespace MB.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using Base;

    public class Monument : BaseModel<int>
    {
        public Monument()
        {
            this.MonumentReviews = new HashSet<MonumentReview>();
            this.MonumentComments = new HashSet<MonumentComment>();
        }

        public string Name { get; set; }

        public string  Description { get; set; }

        public double OverallRating => this.MonumentReviews.Average(x => (int)x.Rating);

        public int OblastId { get; set; }
        public virtual Oblast Oblast { get; set; }

        public virtual IEnumerable<MonumentReview> MonumentReviews { get; set; }

        public virtual IEnumerable<MonumentComment> MonumentComments { get; set; }
    }
}
