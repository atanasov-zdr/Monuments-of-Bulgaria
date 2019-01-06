namespace MB.Tests.Services.Hotels
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
    using MB.Services.Contracts.Oblasts;
    using MB.Services.Hotels;
    using MB.Services.Oblasts;
    using Models.Hotels;
    using Models.Oblasts;
    using Profiles;
    using ViewModels.Hotels;

    [Collection(GlobalConstants.TestClassesCollectionName)]
    public class HotelsServiceTests
    {
        private readonly IServiceProvider provider;
        private readonly MbDbContext dbContext;
        private readonly IHotelsService hotelsService;

        public HotelsServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<MbDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IHotelsService, HotelsService>();
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
            this.hotelsService = this.provider.GetService<IHotelsService>();
        }

        [Fact]
        public void GetAllOrderedByName_ReturnOnlyNotDeletedHotels()
        {
            int hotelid = 2;
            this.dbContext.Hotels.Add(new Hotel { Id = 1, IsDeleted = true });
            this.dbContext.Hotels.Add(new Hotel { Id = hotelid });
            this.dbContext.SaveChanges();

            IQueryable<Hotel> result = this.hotelsService.GetAllOrderedByName();

            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(hotelid);
        }

        [Fact]
        public void GetAllOrderedByName_ReturnHotelsOrderedByName()
        {
            string firstHotelName = "aaa";
            string lastHotelName = "zzz";
            this.dbContext.Hotels.Add(new Hotel { Name = lastHotelName });
            this.dbContext.Hotels.Add(new Hotel { Name = firstHotelName });
            this.dbContext.SaveChanges();

            IQueryable<Hotel> result = this.hotelsService.GetAllOrderedByName();
            result.First().Name.ShouldBe(firstHotelName);
            result.Last().Name.ShouldBe(lastHotelName);
        }

        [Fact]
        public void GetAllForOblastOrderedByName_ReturnOnlyHotelsInGivenOblast()
        {
            int oblastId = 1;
            int hotelId = 2;
            this.dbContext.Hotels.Add(new Hotel { Id = 3, OblastId = 4 });
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId, OblastId = oblastId });
            this.dbContext.SaveChanges();

            IQueryable<Hotel> result = this.hotelsService.GetAllForOblastOrderedByName(oblastId);
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(hotelId);
        }

        [Fact]
        public void GetAllForOblastOrderedByName_ReturnOnlyNotDeletedHotels()
        {
            int oblastId = 1;
            int hotelId = 2;
            this.dbContext.Hotels.Add(new Hotel { Id = 3, OblastId = oblastId, IsDeleted = true });
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId, OblastId = oblastId });
            this.dbContext.SaveChanges();

            IQueryable<Hotel> result = this.hotelsService.GetAllForOblastOrderedByName(oblastId);
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(hotelId);
        }

        [Fact]
        public void GetAllForOblastOrderedByName_ReturnHotelsOrderedByName()
        {
            string firstHotelName = "aaa";
            string lastHotelName = "zzz";
            int oblastId = 1;
            this.dbContext.Hotels.Add(new Hotel { Name = lastHotelName, OblastId = oblastId });
            this.dbContext.Hotels.Add(new Hotel { Name = firstHotelName, OblastId = oblastId });
            this.dbContext.SaveChanges();

            IQueryable<Hotel> result = this.hotelsService.GetAllForOblastOrderedByName(oblastId);
            result.First().Name.ShouldBe(firstHotelName);
            result.Last().Name.ShouldBe(lastHotelName);
        }

        [Fact]
        public void GetById_ReturnCorrectHotel()
        {
            int hotelId = 2;
            this.dbContext.Hotels.Add(new Hotel { Id = 1 });
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId });
            this.dbContext.SaveChanges();

            int result = this.hotelsService.GetById(hotelId).Id;
            result.ShouldBe(hotelId);
        }

        [Fact]
        public void GetById_ThrowExceptionForNotExistingHotelWithGivenId()
        {
            Action testCode = () => this.hotelsService.GetById(1);
            testCode.ShouldThrow<HotelNullException>();
        }

        [Fact]
        public void GetById_ThrowExceptionForDeletedHotel()
        {
            int hotelId = 1;
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId, IsDeleted = true });
            this.dbContext.SaveChanges();

            Action testCode = () => this.hotelsService.GetById(hotelId);
            testCode.ShouldThrow<HotelDeletedException>();
        }

        [Fact]
        public void GetNameById_ReturnCorrectName()
        {
            int hotelId = 1;
            string hotelName = "test";
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId, Name = hotelName });
            this.dbContext.SaveChanges();

            string result = this.hotelsService.GetNameById(hotelId);
            result.ShouldBe(hotelName);
        }

        [Fact]
        public void GetNameById_ThrowExceptionIfHotelNameIsNull()
        {
            int hotelId = 1;
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId });
            this.dbContext.SaveChanges();

            Action testCode = () => this.hotelsService.GetNameById(hotelId);
            testCode.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Add_ShouldCreateNewHotelCorrectly()
        {
            string address = "testAddress";
            string description = "testDescription";
            string name = "testName";
            string phoneNumber = "testPhoneNumner";
            int stars = 1;

            int oblastId = 1;
            this.dbContext.Oblasts.Add(new Oblast { Id = oblastId });
            this.dbContext.SaveChanges();

            var model = new HotelAddViewModel
            {
                Address = address,
                Description = description,
                Name = name,
                PhoneNumber = phoneNumber,
                Stars = stars,
                SelectedOblastId = oblastId,                
            };

            string imageUrl = "testUrl";
            string imagesDirectory = "wwwroot/images/hotels/";
            string imagesFolderName = "hotels";
            var mockedImagesUploader = new Mock<ImagesUploader>(null);
            mockedImagesUploader
                .Setup(x => x.Upload(null, imagesDirectory, imagesFolderName))
                .Returns(imageUrl);

            typeof(HotelsService)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(x => x.FieldType == typeof(ImagesUploader))
                .SetValue(this.hotelsService, mockedImagesUploader.Object);

            this.hotelsService.Add(model);

            Hotel result = this.dbContext.Hotels.First();
            result.ShouldSatisfyAllConditions
            (
                () => result.Address.ShouldBe(address),
                () => result.Description.ShouldBe(description),
                () => result.Name.ShouldBe(name),
                () => result.PhoneNumber.ShouldBe(phoneNumber),
                () => result.Stars.ShouldBe(stars),
                () => result.OblastId.ShouldBe(oblastId),
                () => result.ImageUrl.ShouldBe(imageUrl)
            );
        }
        
        [Fact]
        public void Add_ThrowExceptionIfOblastWithGivenIdDoNotExist()
        {
            var model = new HotelAddViewModel { SelectedOblastId = 1 };
            Action testCode = () => this.hotelsService.Add(model);
            testCode.ShouldThrow<OblastNullException>();
        }

        [Fact]
        public void Delete_SetHotelIsDeletedToTrue()
        {
            int hotelId = 1;
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId });
            this.dbContext.SaveChanges();

            this.hotelsService.Delete(hotelId);

            bool result = this.dbContext.Hotels.First().IsDeleted;
            result.ShouldBe(true);
        }

        [Fact]
        public void Update_UpdateGivenHotelCorrectly()
        {
            int hotelId = 3;
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId });
            this.dbContext.SaveChanges();

            string address = "testAddress";
            string description = "testDescription";
            string name = "testName";
            string phoneNumber = "testPhoneNumner";
            int stars = 1;

            int oblastId = 1;
            this.dbContext.Oblasts.Add(new Oblast { Id = oblastId });
            this.dbContext.SaveChanges();

            var model = new HotelEditViewModel
            {
                Id = hotelId,
                Address = address,
                Description = description,
                Name = name,
                PhoneNumber = phoneNumber,
                Stars = stars,
                SelectedOblastId = oblastId,
            };

            this.hotelsService.Update(model);

            Hotel result = this.dbContext.Hotels.First();
            result.ShouldSatisfyAllConditions
            (
                () => result.Address.ShouldBe(address),
                () => result.Description.ShouldBe(description),
                () => result.Name.ShouldBe(name),
                () => result.PhoneNumber.ShouldBe(phoneNumber),
                () => result.Stars.ShouldBe(stars),
                () => result.OblastId.ShouldBe(oblastId)
            );
        }
        
        [Fact]
        public void Update_ThrowExceptionIfOblastWithGivenIdDoNotExist()
        {
            var model = new HotelEditViewModel { SelectedOblastId = 1 };
            Action testCode = () => this.hotelsService.Update(model);
            testCode.ShouldThrow<OblastNullException>();
        }

        [Fact]
        public void CheckExistById_ReturnTrueIfHotelWithGivenIdExist()
        {
            int hotelId = 1;
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId });
            this.dbContext.SaveChanges();

            bool result = this.hotelsService.CheckExistById(hotelId);
            result.ShouldBe(true);
        }

        [Fact]
        public void CheckExistById_ReturnFalseIfHotelWithGivenIdDoNotExist()
        {
            bool result = this.hotelsService.CheckExistById(1);
            result.ShouldBe(false);
        }
    }
}
