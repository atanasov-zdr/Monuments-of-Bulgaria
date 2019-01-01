namespace MB.Profiles
{
    using System.Linq;

    using AutoMapper;

    using Models.Enums;
    using Models.Hotels;
    using ViewModels.Hotels;
    using ViewModels.Hotels.HotelComments;
    using ViewModels.Hotels.HotelReviews;

    public class HotelsProfile : Profile
    {
        public HotelsProfile()
        {
            base.CreateMap<HotelComment, HotelCommentViewModel>()
                .ForMember(dest => dest.IsLiked, opts => opts.Ignore());

            base.CreateMap<Hotel, HotelEditViewModel>()
                .ForMember(dest => dest.SelectedOblastId, opts => opts.MapFrom(x => x.OblastId))
                .ReverseMap();

            base.CreateMap<HotelAddViewModel, Hotel>()
                .ForMember(dest => dest.OblastId, opts => opts.MapFrom(x => x.SelectedOblastId));

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
                    opts => opts.MapFrom(x => x.HotelReviews.Where(y => y.Rating == Rating.Terrible).Count()))
                .ForMember(dest => dest.TimeOfYearSpring,
                    opts => opts.MapFrom(x => x.HotelReviews.Where(y => y.TimeOfYear == Season.Spring).Count()))
                .ForMember(dest => dest.TimeOfYearSummer,
                    opts => opts.MapFrom(x => x.HotelReviews.Where(y => y.TimeOfYear == Season.Summer).Count()))
                .ForMember(dest => dest.TimeOfYearAutumn,
                    opts => opts.MapFrom(x => x.HotelReviews.Where(y => y.TimeOfYear == Season.Autumn).Count()))
                .ForMember(dest => dest.TimeOfYearWinter,
                    opts => opts.MapFrom(x => x.HotelReviews.Where(y => y.TimeOfYear == Season.Winter).Count()))
                .ForMember(dest => dest.TravellerTypeFamilies,
                    opts => opts.MapFrom(x => x.HotelReviews.Where(y => y.TravellerType == TravellerType.Families).Count()))
                .ForMember(dest => dest.TravellerTypeCouples,
                    opts => opts.MapFrom(x => x.HotelReviews.Where(y => y.TravellerType == TravellerType.Couples).Count()))
                .ForMember(dest => dest.TravellerTypeSolo,
                    opts => opts.MapFrom(x => x.HotelReviews.Where(y => y.TravellerType == TravellerType.Solo).Count()))
                .ForMember(dest => dest.TravellerTypeBusiness,
                    opts => opts.MapFrom(x => x.HotelReviews.Where(y => y.TravellerType == TravellerType.Business).Count()))
                .ForMember(dest => dest.TravellerTypeFriends,
                    opts => opts.MapFrom(x => x.HotelReviews.Where(y => y.TravellerType == TravellerType.Friends).Count()));
        }
    }
}
