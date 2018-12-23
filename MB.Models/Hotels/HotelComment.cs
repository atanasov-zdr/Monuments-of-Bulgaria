namespace MB.Models.Hotels
{
    using System;
    using System.Collections.Generic;

    using Base;

    public class HotelComment : BaseModel<int>
    {
        public HotelComment()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.Likes = new HashSet<HotelCommentLike>();
        }

        public string Content { get; set; }
        
        public DateTime CreatedOn { get; set; }

        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        public string UserId { get; set; }
        public virtual MbUser User { get; set; }

        public virtual IEnumerable<HotelCommentLike> Likes { get; set; }
    }
}
