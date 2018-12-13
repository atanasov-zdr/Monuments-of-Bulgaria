namespace MB.Profiles
{
    using AutoMapper;

    using Models;
    using ViewModels.Oblasts;

    public class OblastsProfile : Profile
    {
        public OblastsProfile()
        {
            base.CreateMap<OblastSeedViewModel, Oblast>();
        }
    }
}
