namespace MB.ViewModels.Monuments
{
    public class MonumentDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double OverallRating { get; set; }

        public int ReviewsCount { get; set; }

        public int CommentsCount { get; set; }
    }
}
