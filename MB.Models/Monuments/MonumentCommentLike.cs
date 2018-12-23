namespace MB.Models.Monuments
{
    using Base;

    public class MonumentCommentLike : BaseModel<int>
    {
        public int MonumentCommentId { get; set; }
        public virtual MonumentComment MonumentComment { get; set; }

        public string UserId { get; set; }
        public virtual MbUser User { get; set; }
    }
}
