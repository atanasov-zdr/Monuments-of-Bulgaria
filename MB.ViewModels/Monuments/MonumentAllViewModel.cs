﻿namespace MB.ViewModels.Monuments
{
    using Mapping.Contracts;
    using Models.Monuments;

    public class MonumentAllViewModel : IMapFrom<Monument>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int OblastId { get; set; }

        public string OblastName { get; set; }

        public string ImageUrl { get; set; }
    }
}
