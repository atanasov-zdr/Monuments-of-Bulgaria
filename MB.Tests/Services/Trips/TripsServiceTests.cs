namespace MB.Tests.Services.Trips
{
    using System;
    using System.Reflection;
    using System.Linq;

    using AutoMapper;

    using CloudinaryDotNet;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using Shouldly;

    using Xunit;

    using Common;
    using Common.Exceptions;
    using Common.Utilities;
    using Data;
    using MB.Services.Contracts.Hotels;
    using MB.Services.Contracts.Monuments;
    using MB.Services.Contracts.Oblasts;
    using MB.Services.Contracts.Trips;
    using MB.Services.Contracts.Users;
    using MB.Services.Hotels;
    using MB.Services.Monuments;
    using MB.Services.Oblasts;
    using MB.Services.Trips;
    using MB.Services.Users;
    using Models;
    using Models.Hotels;
    using Models.Monuments;
    using Models.Trips;
    using Profiles;
    using ViewModels.Trips;

    [Collection(GlobalConstants.TestClassesCollectionName)]
    public class TripsServiceTests
    {
        private readonly IServiceProvider provider;
        private readonly MbDbContext dbContext;
        private readonly ITripsService tripsService;

        public TripsServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<MbDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<ITripsService, TripsService>();
            services.AddScoped<IMonumentsService, MonumentsService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IHotelsService, HotelsService>();
            services.AddScoped<IOblastsService, OblastsService>();

            Mapper.Reset();
            Mapper.Initialize(config => config.AddProfile(typeof(TripsProfile)));
            services.AddScoped<IMapper>(service => Mapper.Instance);

            var account = new Account(GlobalConstants.CloudinaryName,
                GlobalConstants.CloudinaryKey, GlobalConstants.CloudinarySecret);
            services.AddScoped(x => new Cloudinary(account));
            services.AddScoped<ImagesUploader>();

            this.provider = services.BuildServiceProvider();
            this.dbContext = this.provider.GetService<MbDbContext>();
            this.tripsService = this.provider.GetService<ITripsService>();
        }

        [Fact]
        public void Create_ShouldCreateNewTripCorrectly()
        {
            int monumentId = 1;
            int hotelId = 2;
            string username = "testUsername";
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId });
            this.dbContext.Users.Add(new MbUser { UserName = username });
            this.dbContext.SaveChanges();

            string description = "testDescription";
            string name = "testName";
            var model = new TripCreateViewModel
            {
                Description = description,
                Name = name,
                SelectedMonumentId = monumentId,
                SelectedHotelId = hotelId,
            };

            string imageUrl = "testUrl";
            string imagesDirectory = "wwwroot/images/trips/";
            string imagesFolderName = "trips";
            var mockedImagesUploader = new Mock<ImagesUploader>(null);
            mockedImagesUploader
                .Setup(x => x.Upload(null, imagesDirectory, imagesFolderName))
                .Returns(imageUrl);

            typeof(TripsService)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(x => x.FieldType == typeof(ImagesUploader))
                .SetValue(this.tripsService, mockedImagesUploader.Object);

            this.tripsService.Create(model, username);

            Trip result = this.dbContext.Trips.First();
            result.ShouldSatisfyAllConditions
            (
                () => result.Description.ShouldBe(description),
                () => result.Name.ShouldBe(name),
                () => result.ImageUrl.ShouldBe(imageUrl),
                () => result.MonumentId.ShouldBe(monumentId),
                () => result.HotelId.ShouldBe(hotelId)
            );
        }

        [Fact]
        public void Create_ThrowExceptionIfMonumentWithGivenIdDoNotExist()
        {
            var model = new TripCreateViewModel { SelectedMonumentId = 1 };
            Action testCode = () => this.tripsService.Create(model, "testUsername");
            testCode.ShouldThrow<MonumentNullException>();
        }

        [Fact]
        public void Create_ThrowExceptionIfHotelWithGivenIdDoNotExist()
        {
            int monumentId = 1;
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.SaveChanges();

            var model = new TripCreateViewModel { SelectedMonumentId = monumentId, SelectedHotelId = 2 };
            Action testCode = () => this.tripsService.Create(model, "testUSername");
            testCode.ShouldThrow<HotelNullException>();
        }

        [Fact]
        public void GetAllForUser_ReturnOnlyTripsForGivenUser()
        {
            string username = "needed";
            int tripId = 1;
            this.dbContext.Trips.Add(new Trip { Id = 2, User = new MbUser { UserName = "testUsername" } });
            this.dbContext.Trips.Add(new Trip { Id = tripId, User = new MbUser { UserName = username } });
            this.dbContext.SaveChanges();

            IQueryable<Trip> result = this.tripsService.GetAllForUser(username);
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(tripId);
        }

        [Fact]
        public void GetAllForUser_ReturnOnlyNotDeletedTrips()
        {
            string username = "needed";
            int tripId = 1;
            this.dbContext.Trips.Add(new Trip { Id = 2, User = new MbUser { UserName = username }, IsDeleted = true });
            this.dbContext.Trips.Add(new Trip { Id = tripId, User = new MbUser { UserName = username } });
            this.dbContext.SaveChanges();

            IQueryable<Trip> result = this.tripsService.GetAllForUser(username);
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(tripId);
        }
        
        [Fact]
        public void GetById_ReturnCorrectTrip()
        {
            int tripId = 1;
            this.dbContext.Trips.Add(new Trip { Id = 2 });
            this.dbContext.Trips.Add(new Trip { Id = tripId });
            this.dbContext.SaveChanges();

            int result = this.tripsService.GetById(tripId).Id;
            result.ShouldBe(tripId);
        }

        [Fact]
        public void GetById_ThrowExceptionForNotExistingTripWithGivenId()
        {
            Action testCode = () => this.tripsService.GetById(1);
            testCode.ShouldThrow<TripNullException>();
        }

        [Fact]
        public void GetById_ThrowExceptionForDeletedTrip()
        {
            int tripId = 1;
            this.dbContext.Trips.Add(new Trip { Id = tripId, IsDeleted = true });
            this.dbContext.SaveChanges();

            Action testCode = () => this.tripsService.GetById(tripId);
            testCode.ShouldThrow<TripDeletedException>();
        }

        [Fact]
        public void CheckForTripOwn_ReturnTrueIfGivenUserOwnGivenTrip()
        {
            int tripId = 1;
            string username = "testUsername";
            this.dbContext.Trips.Add(new Trip { Id = tripId, User = new MbUser { UserName = username } });
            this.dbContext.SaveChanges();

            bool result = this.tripsService.CheckForTripOwn(tripId, username);
            result.ShouldBe(true);
        }

        [Fact]
        public void CheckForTripOwn_ReturnFalseIfGivenUserDoNotOwnGivenTrip()
        {
            int tripId = 1;
            this.dbContext.Trips.Add(new Trip { Id = tripId, User = new MbUser { UserName = "test" } });
            this.dbContext.SaveChanges();

            bool result = this.tripsService.CheckForTripOwn(tripId, "needed");
            result.ShouldBe(false
);
        }

        [Fact]
        public void Delete_SetTripIsDeletedToTrue()
        {
            int tripId = 1;
            this.dbContext.Trips.Add(new Trip { Id = tripId });
            this.dbContext.SaveChanges();

            this.tripsService.Delete(tripId);

            bool result = this.dbContext.Trips.First().IsDeleted;
            result.ShouldBe(true);
        }

        [Fact]
        public void Update_UpdateGivenTripCorrectly()
        {
            int tripId = 1;
            int hotelId = 2;
            int monumentId = 3;
            this.dbContext.Trips.Add(new Trip { Id = tripId });
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId });
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.SaveChanges();
            
            string description = "testDescription";
            string name = "testName";           
            var model = new TripEditViewModel
            {
                Id = tripId,
                Description = description,
                Name = name,
                SelectedHotelId = hotelId,
                SelectedMonumentId = monumentId,
            };

            this.tripsService.Update(model);

            Trip result = this.dbContext.Trips.First();
            result.ShouldSatisfyAllConditions
            (
                () => result.Description.ShouldBe(description),
                () => result.Name.ShouldBe(name),
                () => result.HotelId.ShouldBe(hotelId),
                () => result.MonumentId.ShouldBe(monumentId)
            );
        }
    }
}
