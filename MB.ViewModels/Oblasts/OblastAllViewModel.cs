namespace MB.ViewModels.Oblasts
{
    using Mapping.Contracts;
    using Models.Oblasts;

    public class OblastAllViewModel : IMapFrom<Oblast>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
