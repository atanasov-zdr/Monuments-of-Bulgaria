namespace MB.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using Base;

    public class Hotel : BaseModel<int>
    {
        public Hotel()
        {
            this.HotelReviews = new HashSet<HotelReview>();
            this.HotelComments = new HashSet<HotelComment>();
        }

        public string Name { get; set; }

        public int Stars { get; set; }

        public string Description { get; set; }

        public double OverallRating => this.HotelReviews.Average(x => (int)x.Rating);

        public int OblastId { get; set; }
        public virtual Oblast Oblast { get; set; }

        public virtual IEnumerable<HotelReview> HotelReviews { get; set; }

        public virtual IEnumerable<HotelComment> HotelComments { get; set; }
    }
}
