namespace MB.Profiles
{
    using System.Linq;

    using AutoMapper;
    
    using Models.Enums;
    using Models.Hotels;
    using Models.Monuments;
    using ViewModels.Hotels;
    using ViewModels.Hotels.HotelReviews;
    using ViewModels.Monuments;
    using ViewModels.Monuments.MonumentReviews;

    public class MbProfiles : Profile
    {
        public MbProfiles()
        {
            base.CreateMap<Monument, MonumentDetailsViewModel>()
                .ForMember(dest => dest.Comments, opts => opts.Ignore());

            base.CreateMap<MonumentReviewWriteViewModel, MonumentReview>()
                .ForMember(dest => dest.CreatedOn, opts => opts.Ignore());

            base.CreateMap<Monument, MonumentReviewsViewModel>()
                .ForMember(dest => dest.RatingExcellent,
                    opts => opts.MapFrom(x => x.MonumentReviews.Where(y => y.Rating == Rating.Excellent).Count()))
                .ForMember(dest => dest.RatingVeryGood,
                    opts => opts.MapFrom(x => x.MonumentReviews.Where(y => y.Rating == Rating.VeryGood).Count()))
                .ForMember(dest => dest.RatingAverage,
                    opts => opts.MapFrom(x => x.MonumentReviews.Where(y => y.Rating == Rating.Average).Count()))
                .ForMember(dest => dest.RatingPoor,
                    opts => opts.MapFrom(x => x.MonumentReviews.Where(y => y.Rating == Rating.Poor).Count()))
                .ForMember(dest => dest.RatingTerrible,
                    opts => opts.MapFrom(x => x.MonumentReviews.Where(y => y.Rating == Rating.Terrible).Count()))
                .ForMember(dest => dest.TimeOfYearSpring,
                    opts => opts.MapFrom(x => x.MonumentReviews.Where(y => y.TimeOfYear == Season.Spring).Count()))
                .ForMember(dest => dest.TimeOfYearSummer,
                    opts => opts.MapFrom(x => x.MonumentReviews.Where(y => y.TimeOfYear == Season.Summer).Count()))
                .ForMember(dest => dest.TimeOfYearAutumn,
                    opts => opts.MapFrom(x => x.MonumentReviews.Where(y => y.TimeOfYear == Season.Autumn).Count()))
                .ForMember(dest => dest.TimeOfYearWinter,
                    opts => opts.MapFrom(x => x.MonumentReviews.Where(y => y.TimeOfYear == Season.Winter).Count()))
                .ForMember(dest => dest.TravellerTypeFamilies,
                    opts => opts.MapFrom(x => x.MonumentReviews.Where(y => y.TravellerType == TravellerType.Families).Count()))
                .ForMember(dest => dest.TravellerTypeCouples,
                    opts => opts.MapFrom(x => x.MonumentReviews.Where(y => y.TravellerType == TravellerType.Couples).Count()))
                .ForMember(dest => dest.TravellerTypeSolo,
                    opts => opts.MapFrom(x => x.MonumentReviews.Where(y => y.TravellerType == TravellerType.Solo).Count()))
                .ForMember(dest => dest.TravellerTypeBusiness,
                    opts => opts.MapFrom(x => x.MonumentReviews.Where(y => y.TravellerType == TravellerType.Business).Count()))
                .ForMember(dest => dest.TravellerTypeFriends,
                    opts => opts.MapFrom(x => x.MonumentReviews.Where(y => y.TravellerType == TravellerType.Friends).Count()));
            
            base.CreateMap<Hotel, HotelDetailsViewModel>()
                .ForMember(dest => dest.Comments, opts => opts.Ignore());

            base.CreateMap<HotelReviewWriteViewModel, HotelReview>()
                .ForMember(dest => dest.CreatedOn, opts => opts.Ignore());

            base.CreateMap<Hotel, HotelReviewsViewModel>()
                .ForMember(dest => dest.RatingExcellent,
                    opts => opts.MapFrom(x => x.HotelReviews.Where(y => y.Rating == Rating.Excellent).Count()))
                .ForMember(dest => dest.RatingVeryGood,
                    opts => opts.MapFrom(x => x.HotelReviews.Where(y => y.Rating == Rating.VeryGood).Count()))
                .ForMember(dest => dest.RatingAverage,
                    opts => opts.MapFrom(x => x.HotelReviews.Where(y => y.Rating == Rating.Average).Count()))
                .ForMember(dest => dest.RatingPoor,
                    opts => opts.MapFrom(x => x.HotelReviews.Where(y => y.Rating == Rating.Poor).Count()))
                .ForMember(dest => dest.RatingTerrible,
                    opts => opts.MapFrom(x => x.HotelReviews.Where(y => y.Rating == Rating.Terrible).Count()));
        }
    }
}
