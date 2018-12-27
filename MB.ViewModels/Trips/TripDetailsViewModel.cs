namespace MB.ViewModels.Trips
{
    public class TripDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MonumentId { get; set; }

        public string MonumentName { get; set; }
        
        public int MonumentOblastId { get; set; }

        public string MonumentOblastName { get; set; }

        public int HotelId { get; set; }

        public string HotelName { get; set; }

        public int HotelOblastId { get; set; }

        public string HotelOblastName { get; set; }
    }
}
