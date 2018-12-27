namespace MB.ViewModels.Hotels
{
    using Mapping.Contracts;
    using Models.Hotels;

    public class HotelAllViewModel : IMapFrom<Hotel>
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int OblastId { get; set; }

        public string OblastName { get; set; }
    }
}
