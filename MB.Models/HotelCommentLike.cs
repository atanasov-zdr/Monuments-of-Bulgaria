namespace MB.Models
{
    using Base;

    public class HotelCommentLike : BaseModel<int>
    {
        public int HotelCommentId { get; set; }
        public virtual HotelComment HotelComment { get; set; }

        public string UserId { get; set; }
        public virtual MbUser User { get; set; }
    }
}
