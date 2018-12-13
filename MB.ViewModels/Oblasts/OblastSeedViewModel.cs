namespace MB.ViewModels.Oblasts
{
    using Mapping.Contracts;
    using Models;

    public class OblastSeedViewModel : IMapTo<Oblast>
    {
        public string Name { get; set; }

        public double LandArea { get; set; }

        public int Population { get; set; }

        public double PopulationDensity { get; set; }

        public int MunicipalitiesCount { get; set; }

        public string Description { get; set; }
    }
}
