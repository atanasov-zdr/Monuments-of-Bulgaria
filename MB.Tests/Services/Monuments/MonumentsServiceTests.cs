namespace MB.Tests.Services.Monuments
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
    using MB.Services.Contracts.Monuments;
    using MB.Services.Contracts.Oblasts;
    using MB.Services.Monuments;
    using MB.Services.Oblasts;
    using Models.Monuments;
    using Models.Oblasts;
    using Profiles;
    using ViewModels.Monuments;

    [Collection(GlobalConstants.TestClassesCollectionName)]
    public class MonumentsServiceTests
    {
        private readonly IServiceProvider provider;
        private readonly MbDbContext dbContext;
        private readonly IMonumentsService monumentsService;

        public MonumentsServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<MbDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IMonumentsService, MonumentsService>();
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
            this.monumentsService = this.provider.GetService<IMonumentsService>();
        }

        [Fact]
        public void GetAllOrderedByName_ReturnOnlyNotDeletedMonuments()
        {
            int monumentid = 2;
            this.dbContext.Monuments.Add(new Monument { Id = 1, IsDeleted = true });
            this.dbContext.Monuments.Add(new Monument { Id = monumentid });
            this.dbContext.SaveChanges();

            IQueryable<Monument> result = this.monumentsService.GetAllOrderedByName();

            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(monumentid);
        }

        [Fact]
        public void GetAllOrderedByName_ReturnMonumentsOrderedByName()
        {
            string firstMonumentName = "aaa";
            string lastMonumentName = "zzz";
            this.dbContext.Monuments.Add(new Monument { Name = lastMonumentName });
            this.dbContext.Monuments.Add(new Monument { Name = firstMonumentName });
            this.dbContext.SaveChanges();

            IQueryable<Monument> result = this.monumentsService.GetAllOrderedByName();
            result.First().Name.ShouldBe(firstMonumentName);
            result.Last().Name.ShouldBe(lastMonumentName);
        }

        [Fact]
        public void GetAllForOblastOrderedByName_ReturnOnlyMonumentsInGivenOblast()
        {
            int oblastId = 1;
            int monumentId = 2;
            this.dbContext.Monuments.Add(new Monument { Id = 3, OblastId = 4 });
            this.dbContext.Monuments.Add(new Monument { Id = monumentId, OblastId = oblastId });
            this.dbContext.SaveChanges();

            IQueryable<Monument> result = this.monumentsService.GetAllForOblastOrderedByName(oblastId);
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(monumentId);
        }

        [Fact]
        public void GetAllForOblastOrderedByName_ReturnOnlyNotDeletedMonuments()
        {
            int oblastId = 1;
            int monumentId = 2;
            this.dbContext.Monuments.Add(new Monument { Id = 3, OblastId = oblastId, IsDeleted = true });
            this.dbContext.Monuments.Add(new Monument { Id = monumentId, OblastId = oblastId });
            this.dbContext.SaveChanges();

            IQueryable<Monument> result = this.monumentsService.GetAllForOblastOrderedByName(oblastId);
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(monumentId);
        }

        [Fact]
        public void GetAllForOblastOrderedByName_ReturnMonumentsOrderedByName()
        {
            string firstMonumentName = "aaa";
            string lastMonumentName = "zzz";
            int oblastId = 1;
            this.dbContext.Monuments.Add(new Monument { Name = lastMonumentName, OblastId = oblastId });
            this.dbContext.Monuments.Add(new Monument { Name = firstMonumentName, OblastId = oblastId });
            this.dbContext.SaveChanges();

            IQueryable<Monument> result = this.monumentsService.GetAllForOblastOrderedByName(oblastId);
            result.First().Name.ShouldBe(firstMonumentName);
            result.Last().Name.ShouldBe(lastMonumentName);
        }

        [Fact]
        public void GetById_ReturnCorrectMonument()
        {
            int monumentId = 2;
            this.dbContext.Monuments.Add(new Monument { Id = 1 });
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.SaveChanges();

            int result = this.monumentsService.GetById(monumentId).Id;
            result.ShouldBe(monumentId);
        }

        [Fact]
        public void GetById_ThrowExceptionForNotExistingMonumentWithGivenId()
        {
            Action testCode = () => this.monumentsService.GetById(1);
            testCode.ShouldThrow<MonumentNullException>();
        }

        [Fact]
        public void GetById_ThrowExceptionForDeletedMonument()
        {
            int monumentId = 1;
            this.dbContext.Monuments.Add(new Monument { Id = monumentId, IsDeleted = true });
            this.dbContext.SaveChanges();

            Action testCode = () => this.monumentsService.GetById(monumentId);
            testCode.ShouldThrow<MonumentDeletedException>();
        }

        [Fact]
        public void GetNameById_ReturnCorrectName()
        {
            int monumentId = 1;
            string MonumentName = "test";
            this.dbContext.Monuments.Add(new Monument { Id = monumentId, Name = MonumentName });
            this.dbContext.SaveChanges();

            string result = this.monumentsService.GetNameById(monumentId);
            result.ShouldBe(MonumentName);
        }

        [Fact]
        public void GetNameById_ThrowExceptionIfMonumentNameIsNull()
        {
            int monumentId = 1;
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.SaveChanges();

            Action testCode = () => this.monumentsService.GetNameById(monumentId);
            testCode.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Add_ShouldCreateNewMonumentCorrectly()
        {
            string description = "testDescription";
            string name = "testName";

            int oblastId = 1;
            this.dbContext.Oblasts.Add(new Oblast { Id = oblastId });
            this.dbContext.SaveChanges();

            var model = new MonumentAddViewModel
            {
                Description = description,
                Name = name,
                SelectedOblastId = oblastId,                
            };

            string imageUrl = "testUrl";
            string imagesDirectory = "wwwroot/images/monuments/";
            string imagesFolderName = "monuments";
            var mockedImagesUploader = new Mock<ImagesUploader>(null);
            mockedImagesUploader
                .Setup(x => x.Upload(null, imagesDirectory, imagesFolderName))
                .Returns(imageUrl);

            typeof(MonumentsService)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(x => x.FieldType == typeof(ImagesUploader))
                .SetValue(this.monumentsService, mockedImagesUploader.Object);

            this.monumentsService.Add(model);

            Monument result = this.dbContext.Monuments.First();
            result.ShouldSatisfyAllConditions
            (
                () => result.Description.ShouldBe(description),
                () => result.Name.ShouldBe(name),
                () => result.OblastId.ShouldBe(oblastId),
                () => result.ImageUrl.ShouldBe(imageUrl)
            );
        }
        
        [Fact]
        public void Add_ThrowExceptionIfOblastWithGivenIdDoNotExist()
        {
            var model = new MonumentAddViewModel { SelectedOblastId = 1 };
            Action testCode = () => this.monumentsService.Add(model);
            testCode.ShouldThrow<OblastNullException>();
        }

        [Fact]
        public void Delete_SetMonumentIsDeletedToTrue()
        {
            int monumentId = 1;
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.SaveChanges();

            this.monumentsService.Delete(monumentId);

            bool result = this.dbContext.Monuments.First().IsDeleted;
            result.ShouldBe(true);
        }

        [Fact]
        public void Update_UpdateGivenMonumentCorrectly()
        {
            int monumentId = 3;
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.SaveChanges();
            
            string description = "testDescription";
            string name = "testName";

            int oblastId = 1;
            this.dbContext.Oblasts.Add(new Oblast { Id = oblastId });
            this.dbContext.SaveChanges();

            var model = new MonumentEditViewModel
            {
                Id = monumentId,
                Description = description,
                Name = name,
                SelectedOblastId = oblastId,
            };

            this.monumentsService.Update(model);

            Monument result = this.dbContext.Monuments.First();
            result.ShouldSatisfyAllConditions
            (
                () => result.Description.ShouldBe(description),
                () => result.Name.ShouldBe(name),
                () => result.OblastId.ShouldBe(oblastId)
            );
        }
        
        [Fact]
        public void Update_ThrowExceptionIfOblastWithGivenIdDoNotExist()
        {
            var model = new MonumentEditViewModel { SelectedOblastId = 1 };
            Action testCode = () => this.monumentsService.Update(model);
            testCode.ShouldThrow<OblastNullException>();
        }

        [Fact]
        public void CheckExistById_ReturnTrueIfMonumentWithGivenIdExist()
        {
            int monumentId = 1;
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.SaveChanges();

            bool result = this.monumentsService.CheckExistById(monumentId);
            result.ShouldBe(true);
        }

        [Fact]
        public void CheckExistById_ReturnFalseIfMonumentWithGivenIdDoNotExist()
        {
            bool result = this.monumentsService.CheckExistById(1);
            result.ShouldBe(false);
        }
    }
}
