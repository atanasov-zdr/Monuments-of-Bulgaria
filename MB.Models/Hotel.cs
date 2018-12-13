namespace MB.Models
{
    using Base;

    public class Hotel : BaseModel<int>
    {
        public string Name { get; set; }

        public int OblastId { get; set; }
        public virtual Oblast Oblast { get; set; }
    }
}
