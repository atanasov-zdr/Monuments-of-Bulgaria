namespace MB.ViewModels.Monuments
{
    using Mapping.Contracts;
    using Models;

    public class MonumentAllViewModel : IMapFrom<Monument>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int OblastId { get; set; }

        public string OblastName { get; set; }
    }
}
