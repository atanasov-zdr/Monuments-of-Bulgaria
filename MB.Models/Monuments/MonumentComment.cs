namespace MB.Models.Monuments
{
    using System;
    using System.Collections.Generic;

    using Base;

    public class MonumentComment : BaseModel<int>
    {
        public MonumentComment()
        {
            this.Likes = new HashSet<MonumentCommentLike>();
            this.CreatedOn = DateTime.UtcNow;
        }

        public string Content { get; set; }
        
        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

        public int MonumentId { get; set; }
        public virtual Monument Monument { get; set; }

        public string UserId { get; set; }
        public virtual MbUser User { get; set; }

        public virtual IEnumerable<MonumentCommentLike> Likes { get; set; }
    }
}
