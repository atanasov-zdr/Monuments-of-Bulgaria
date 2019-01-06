namespace MB.Tests.Services.Oblasts
{
    using System;
    using System.Linq;
    
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Shouldly;

    using Xunit;

    using Common;
    using Data;
    using MB.Services.Contracts.Oblasts;
    using MB.Services.Oblasts;
    using Models.Oblasts;

    [Collection(GlobalConstants.TestClassesCollectionName)]
    public class OblastsServiceTests
    {
        private readonly IServiceProvider provider;
        private readonly MbDbContext dbContext;
        private readonly IOblastsService oblastsService;

        public OblastsServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<MbDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IOblastsService, OblastsService>();
            
            this.provider = services.BuildServiceProvider();
            this.dbContext = this.provider.GetService<MbDbContext>();
            this.oblastsService = this.provider.GetService<IOblastsService>();
        }

        [Fact]
        public void GetAllOrderedByName_ReturnAllOblasts()
        {
            this.dbContext.Oblasts.Add(new Oblast());
            this.dbContext.Oblasts.Add(new Oblast());
            this.dbContext.SaveChanges();

            int result = this.oblastsService.GetAllOrderedByName().Count();
            result.ShouldBe(2);
        }

        [Fact]
        public void GetAllOrderedByName_ReturnOblastsOrderedByName()
        {
            string firstOblastName = "aaa";
            string lastOblastName = "zzz";
            this.dbContext.Oblasts.Add(new Oblast { Name = lastOblastName });
            this.dbContext.Oblasts.Add(new Oblast { Name = firstOblastName });
            this.dbContext.SaveChanges();

            IQueryable<Oblast> result = this.oblastsService.GetAllOrderedByName();
            result.First().Name.ShouldBe(firstOblastName);
            result.Last().Name.ShouldBe(lastOblastName);
        }

        [Fact]
        public void CheckExistById_ReturnTrueIfOblastWithGiveIdExist()
        {
            int oblastId = 1;
            this.dbContext.Oblasts.Add(new Oblast { Id = oblastId });
            this.dbContext.SaveChanges();

            bool result = this.oblastsService.CheckExistById(oblastId);
            result.ShouldBe(true);
        }

        [Fact]
        public void CheckExistById_ReturnFalseIfOblastWithGiveIdDoNotExist()
        {
            bool result = this.oblastsService.CheckExistById(1);
            result.ShouldBe(false);
        }
    }
}
