namespace MB.Profiles
{
    using AutoMapper;
    
    using Models.Trips;
    using ViewModels.Trips;

    public class TripsProfile : Profile
    {
        public TripsProfile()
        {
            base.CreateMap<TripCreateViewModel, Trip>()
                .ForMember(dest => dest.HotelId, opts => opts.MapFrom(x => x.SelectedHotelId))
                .ForMember(dest => dest.MonumentId, opts => opts.MapFrom(x => x.SelectedMonumentId));
        }
    }
}
