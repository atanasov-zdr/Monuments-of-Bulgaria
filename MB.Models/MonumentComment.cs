namespace MB.Models
{
    using Base;

    public class MonumentComment : BaseModel<int>
    {
        public string Content { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public int MonumentId { get; set; }
        public virtual Monument Monument { get; set; }

        public string UserId { get; set; }
        public virtual MbUser User { get; set; }
    }
}
