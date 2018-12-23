namespace MB.Services.Hotels
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Contracts.Hotels;
    using Data;
    using Models;
    using Models.Hotels;
    using ViewModels.Hotels.HotelReviews;

    public class HotelReviewsService : IHotelReviewsService
    {
        private readonly MbDbContext dbContext;
        private readonly IMapper mapper;

        public HotelReviewsService(MbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public string GetNameById(int hotelId)
        {
            Hotel hotel = this.dbContext.Hotels.FirstOrDefault(x => x.Id == hotelId);

            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            if (hotel.Name == null)
                throw new ArgumentNullException(nameof(hotel.Name));

            return hotel.Name;
        }

        public void Create(HotelReviewWriteViewModel model, string username)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (username == null)
                throw new ArgumentNullException(nameof(username));

            MbUser user = this.dbContext.Users.FirstOrDefault(x => x.UserName == username);

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            HotelReview hotelReview = this.mapper.Map<HotelReview>(model);
            hotelReview.UserId = user.Id;

            this.dbContext.HotelReviews.Add(hotelReview);
            this.dbContext.SaveChanges();
        }
    }
}
