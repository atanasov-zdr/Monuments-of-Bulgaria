namespace MB.Tests.Services.Hotels
{
    using System;
    using System.Linq;

    using AutoMapper;

    using CloudinaryDotNet;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Shouldly;

    using Xunit;

    using Common;
    using Common.Exceptions;
    using Common.Utilities;
    using Data;
    using MB.Services.Contracts.Hotels;
    using MB.Services.Contracts.Oblasts;
    using MB.Services.Contracts.Users;
    using MB.Services.Hotels;
    using MB.Services.Oblasts;
    using MB.Services.Users;
    using Models;
    using Models.Enums;
    using Models.Hotels;
    using Profiles;
    using ViewModels.Hotels.HotelReviews;

    [Collection(GlobalConstants.TestClassesCollectionName)]
    public class HotelReviewsServiceTests
    {
        private readonly IServiceProvider provider;
        private readonly MbDbContext dbContext;
        private readonly IHotelReviewsService hotelReviewsService;
        
        public HotelReviewsServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<MbDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IHotelReviewsService, HotelReviewsService>();
            services.AddScoped<IHotelsService, HotelsService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IOblastsService, OblastsService>();

            Mapper.Reset();
            Mapper.Initialize(config => config.AddProfile(typeof(HotelsProfile)));
            services.AddScoped<IMapper>(service => Mapper.Instance);

            var account = new Account(GlobalConstants.CloudinaryName,
                GlobalConstants.CloudinaryKey, GlobalConstants.CloudinarySecret);
            services.AddScoped(x => new Cloudinary(account));
            services.AddScoped<ImagesUploader>();

            this.provider = services.BuildServiceProvider();
            this.dbContext = this.provider.GetService<MbDbContext>();
            this.hotelReviewsService = this.provider.GetService<IHotelReviewsService>();
        }

        [Fact]
        public void Create_ShouldCreateNewReviewCorrectly()
        {
            int hotelId = 1;
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId });
            this.dbContext.SaveChanges();

            Rating rating = Rating.Average;
            Season timeOfYear = Season.Autumn;
            TravellerType travellerType = TravellerType.Business;

            var model = new HotelReviewWriteViewModel
            {
                HotelId = hotelId,
                Rating = rating,
                TimeOfYear = timeOfYear,
                TravellerType = travellerType,
            };

            string username = "testUsername";
            this.dbContext.Users.Add(new MbUser { UserName = username });
            this.dbContext.SaveChanges();

            this.hotelReviewsService.Create(model, username);
            HotelReview result = this.dbContext.HotelReviews.First();

            result.ShouldSatisfyAllConditions
            (
                () => result.HotelId.ShouldBe(hotelId),
                () => result.Rating.ShouldBe(rating),
                () => result.TimeOfYear.ShouldBe(timeOfYear),
                () => result.TravellerType.ShouldBe(travellerType),
                () => result.User.UserName.ShouldBe(username)
            );
        }


        [Fact]
        public void Create_ThrowExceptionIfHotelWithGivenIdDoNotExist()
        {
            var model = new HotelReviewWriteViewModel { HotelId = 1 };
            Action testCode = () => this.hotelReviewsService.Create(model, "testUsername");
            testCode.ShouldThrow<HotelNullException>();
        }

        [Fact]
        public void CheckForExistingReview_ReturnTrueIfReviewExist()
        {
            int hotelId = 1;
            string username = "testUsername";
            this.dbContext.HotelReviews.Add(new HotelReview
            {
                Hotel = new Hotel { Id = hotelId },
                User = new MbUser { UserName = username },
            });
            this.dbContext.SaveChanges();

            bool result = this.hotelReviewsService.CheckForExistingReview(hotelId, username);
            result.ShouldBe(true);
        }

        [Fact]
        public void CheckForExistingReview_ReturnFalseIfReviewDoNotExist()
        {
            int hotelId = 1;
            string username = "testUsername";
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId });
            this.dbContext.Users.Add(new MbUser { UserName = username });
            this.dbContext.SaveChanges();

            bool result = this.hotelReviewsService.CheckForExistingReview(hotelId, username);
            result.ShouldBe(false);
        }

    }
}
