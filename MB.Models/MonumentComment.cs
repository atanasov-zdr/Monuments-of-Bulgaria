namespace MB.Models
{
    using System;

    using Base;

    public class MonumentComment : BaseModel<int>
    {
        public MonumentComment()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        public string Content { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public DateTime CreatedOn { get; set; }

        public int MonumentId { get; set; }
        public virtual Monument Monument { get; set; }

        public string UserId { get; set; }
        public virtual MbUser User { get; set; }
    }
}
