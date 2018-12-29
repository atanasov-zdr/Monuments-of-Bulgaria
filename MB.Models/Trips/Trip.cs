namespace MB.Models.Trips
{
    using Base;
    using Models.Monuments;
    using Models.Hotels;

    public class Trip : BaseModel<int>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsDeleted { get; set; }

        public string UserId { get; set; }
        public virtual MbUser User { get; set; }

        public int MonumentId { get; set; }
        public virtual Monument Monument { get; set; }

        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }
    }
}
