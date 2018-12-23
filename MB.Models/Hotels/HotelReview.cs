namespace MB.Models.Hotels
{
    using System;

    using Base;
    using Enums;

    public class HotelReview : BaseModel<int>
    {
        public HotelReview()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        public Rating? Rating { get; set; }

        public Season? TimeOfYear { get; set; }

        public TravellerType? TravellerType { get; set; }

        public DateTime CreatedOn { get; set; }

        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        public string UserId { get; set; }
        public virtual MbUser User { get; set; }
    }
}
