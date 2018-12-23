namespace MB.Models.Oblasts
{
    using System.Collections.Generic;

    using Base;
    using Hotels;
    using Monuments;

    public class Oblast : BaseModel<int>
    {
        public Oblast()
        {
            this.Monuments = new HashSet<Monument>();
            this.Hotels = new HashSet<Hotel>();
        }

        public string Name { get; set; }

        public double LandArea { get; set; }

        public int Population { get; set; }

        public double PopulationDensity { get; set; }

        public int MunicipalitiesCount { get; set; }

        public string Description { get; set; }

        public virtual IEnumerable<Monument> Monuments { get; set; }

        public virtual IEnumerable<Hotel> Hotels { get; set; }
    }
}
