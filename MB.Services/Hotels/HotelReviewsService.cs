namespace MB.Services.Hotels
{
    using System.Linq;

    using AutoMapper;

    using Common.Exceptions;
    using Contracts.Hotels;
    using Contracts.Users;
    using Data;
    using Models;
    using Models.Hotels;
    using ViewModels.Hotels.HotelReviews;

    public class HotelReviewsService : IHotelReviewsService
    {
        private readonly MbDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IUsersService usersService;
        private readonly IHotelsService hotelsService;

        public HotelReviewsService(MbDbContext dbContext, 
            IMapper mapper, 
            IUsersService usersService,
            IHotelsService hotelsService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.usersService = usersService;
            this.hotelsService = hotelsService;
        }

        public void Create(HotelReviewWriteViewModel model, string username)
        {
            if (!this.hotelsService.CheckExistById(model.HotelId))
                throw new HotelNullException();

            HotelReview hotelReview = this.mapper.Map<HotelReview>(model);

            MbUser user = this.usersService.GetByUsername(username);
            hotelReview.User = user;

            this.dbContext.HotelReviews.Add(hotelReview);
            this.dbContext.SaveChanges();
        }

        public bool CheckForExistingReview(int hotelId, string username)
        {
            Hotel hotel = this.hotelsService.GetById(hotelId);
            MbUser user = this.usersService.GetByUsername(username);

            bool result = this.dbContext.HotelReviews.Any(x => x.Hotel == hotel && x.User == user);
            return result;
        }
    }
}
