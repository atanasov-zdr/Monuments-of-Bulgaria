namespace MB.Models
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;

    public class MbUser : IdentityUser
    {
        public MbUser()
        {
            this.MonumentReviews = new HashSet<MonumentReview>();
            this.MonumentComments = new HashSet<MonumentComment>();
            this.MonumentCommentLikes = new HashSet<MonumentCommentLike>();
            this.HotelReviews = new HashSet<HotelReview>();
            this.HotelComments = new HashSet<HotelComment>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public virtual IEnumerable<MonumentReview> MonumentReviews { get; set; }

        public virtual IEnumerable<MonumentComment> MonumentComments { get; set; }

        public virtual IEnumerable<MonumentCommentLike> MonumentCommentLikes { get; set; }

        public virtual IEnumerable<HotelReview> HotelReviews { get; set; }

        public virtual IEnumerable<HotelComment> HotelComments { get; set; }
    }
}
