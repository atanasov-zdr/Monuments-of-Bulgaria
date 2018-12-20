namespace MB.Models
{
    using System;

    using Base;
    using Enums;

    public class MonumentReview : BaseModel<int>
    {
        public MonumentReview()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        public Rating? Rating { get; set; }

        public Season? TimeOfYear { get; set; }

        public TravellerType? TravellerType { get; set; }

        public DateTime CreatedOn { get; set; }

        public int MonumentId { get; set; }
        public virtual Monument Monument { get; set; }

        public string UserId { get; set; }
        public virtual MbUser User { get; set; }       
    }
}
