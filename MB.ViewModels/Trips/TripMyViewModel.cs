namespace MB.ViewModels.Trips
{
    using Mapping.Contracts;
    using Models.Trips;

    public class TripMyViewModel : IMapFrom<Trip>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MonumentName { get; set; }

        public string HotelName { get; set; }
    }
}
