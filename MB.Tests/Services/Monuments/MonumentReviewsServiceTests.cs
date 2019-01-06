namespace MB.Tests.Services.Monuments
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
    using MB.Services.Contracts.Monuments;
    using MB.Services.Contracts.Oblasts;
    using MB.Services.Contracts.Users;
    using MB.Services.Monuments;
    using MB.Services.Oblasts;
    using MB.Services.Users;
    using Models;
    using Models.Enums;
    using Models.Monuments;
    using Profiles;
    using ViewModels.Monuments.MonumentReviews;

    [Collection(GlobalConstants.TestClassesCollectionName)]
    public class MonumentReviewsServiceTests
    {
        private readonly IServiceProvider provider;
        private readonly MbDbContext dbContext;
        private readonly IMonumentReviewsService monumentReviewsService;
        
        public MonumentReviewsServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<MbDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IMonumentReviewsService, MonumentReviewsService>();
            services.AddScoped<IMonumentsService, MonumentsService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IOblastsService, OblastsService>();

            Mapper.Reset();
            Mapper.Initialize(config => config.AddProfile(typeof(MonumentsProfile)));
            services.AddScoped<IMapper>(service => Mapper.Instance);

            var account = new Account(GlobalConstants.CloudinaryName,
                GlobalConstants.CloudinaryKey, GlobalConstants.CloudinarySecret);
            services.AddScoped(x => new Cloudinary(account));
            services.AddScoped<ImagesUploader>();

            this.provider = services.BuildServiceProvider();
            this.dbContext = this.provider.GetService<MbDbContext>();
            this.monumentReviewsService = this.provider.GetService<IMonumentReviewsService>();
        }

        [Fact]
        public void Create_ShouldCreateNewReviewCorrectly()
        {
            int monumentId = 1;
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.SaveChanges();

            Rating rating = Rating.Average;
            Season timeOfYear = Season.Autumn;
            TravellerType travellerType = TravellerType.Business;

            var model = new MonumentReviewWriteViewModel
            {
                MonumentId = monumentId,
                Rating = rating,
                TimeOfYear = timeOfYear,
                TravellerType = travellerType,
            };

            string username = "testUsername";
            this.dbContext.Users.Add(new MbUser { UserName = username });
            this.dbContext.SaveChanges();

            this.monumentReviewsService.Create(model, username);
            MonumentReview result = this.dbContext.MonumentReviews.First();

            result.ShouldSatisfyAllConditions
            (
                () => result.MonumentId.ShouldBe(monumentId),
                () => result.Rating.ShouldBe(rating),
                () => result.TimeOfYear.ShouldBe(timeOfYear),
                () => result.TravellerType.ShouldBe(travellerType),
                () => result.User.UserName.ShouldBe(username)
            );
        }


        [Fact]
        public void Create_ThrowExceptionIfMonumentWithGivenIdDoNotExist()
        {
            var model = new MonumentReviewWriteViewModel { MonumentId = 1 };
            Action testCode = () => this.monumentReviewsService.Create(model, "testUsername");
            testCode.ShouldThrow<MonumentNullException>();
        }

        [Fact]
        public void CheckForExistingReview_ReturnTrueIfReviewExist()
        {
            int monumentId = 1;
            string username = "testUsername";
            this.dbContext.MonumentReviews.Add(new MonumentReview
            {
                Monument = new Monument { Id = monumentId },
                User = new MbUser { UserName = username },
            });
            this.dbContext.SaveChanges();

            bool result = this.monumentReviewsService.CheckForExistingReview(monumentId, username);
            result.ShouldBe(true);
        }

        [Fact]
        public void CheckForExistingReview_ReturnFalseIfReviewDoNotExist()
        {
            int monumentId = 1;
            string username = "testUsername";
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.Users.Add(new MbUser { UserName = username });
            this.dbContext.SaveChanges();

            bool result = this.monumentReviewsService.CheckForExistingReview(monumentId, username);
            result.ShouldBe(false);
        }

    }
}
