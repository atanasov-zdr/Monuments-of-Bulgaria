namespace MB.Tests.Services.Users
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Shouldly;

    using Xunit;

    using Common;
    using Common.Exceptions;
    using Data;
    using MB.Services.Contracts.Users;
    using MB.Services.Users;
    using Models;

    [Collection(GlobalConstants.TestClassesCollectionName)]
    public class UsersServiceTests
    {
        private readonly IServiceProvider provider;
        private readonly MbDbContext dbContext;
        private readonly IUsersService usersService;

        public UsersServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<MbDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IUsersService, UsersService>();

            this.provider = services.BuildServiceProvider();
            this.dbContext = this.provider.GetService<MbDbContext>();
            this.usersService = this.provider.GetService<IUsersService>();
        }

        [Fact]
        public void GetByUsername_ReturnCorrectUser()
        {
            string username = "neededUsername";
            this.dbContext.Users.Add(new MbUser { UserName = "testUsername" });
            this.dbContext.Users.Add(new MbUser { UserName = username });
            this.dbContext.SaveChanges();

            string result = this.usersService.GetByUsername(username).UserName;
            result.ShouldBe(username);
        }

        [Fact]
        public void GetByUsername_ThrowExceptionForNullUsername()
        {
            Action testCode = () => this.usersService.GetByUsername(null);
            testCode.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void GetByUsername_ThrowExceptionIfUserWithGivenUsernameDoNotExist()
        {
            Action testCode = () => this.usersService.GetByUsername("testUsername");
            testCode.ShouldThrow<UserNullException>();
        }
    }
}
