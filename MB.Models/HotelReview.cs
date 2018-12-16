namespace MB.Models
{
    using Base;
    using Enums;

    public class HotelReview : BaseModel<int>
    {
        public Rating Rating { get; set; }
        
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        public string UserId { get; set; }
        public virtual MbUser User { get; set; }
    }
}
