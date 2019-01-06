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
    using Models.Monuments;
    using Profiles;

    [Collection(GlobalConstants.TestClassesCollectionName)]
    public class MonumentCommentsServiceTests
    {
        private readonly IServiceProvider provider;
        private readonly MbDbContext dbContext;
        private readonly IMonumentCommentsService monumentCommentsService;

        public MonumentCommentsServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<MbDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IMonumentCommentsService, MonumentCommentsService>();
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
            this.monumentCommentsService = this.provider.GetService<IMonumentCommentsService>();
        }

        [Fact]
        public void GetById_ReturnCorrectComment()
        {
            int commentId = 2;
            this.dbContext.MonumentComments.Add(new MonumentComment { Id = 1 });
            this.dbContext.MonumentComments.Add(new MonumentComment { Id = commentId });
            this.dbContext.SaveChanges();

            int result = this.monumentCommentsService.GetById(commentId).Id;
            result.ShouldBe(commentId);
        }

        [Fact]
        public void GetById_ThrowExceptionForNotExistingCommentWithGivenId()
        {
            Action testCode = () => this.monumentCommentsService.GetById(2);
            testCode.ShouldThrow<CommentNullException>();
        }

        [Fact]
        public void GetById_ThrowExceptionForDeletedComment()
        {
            int commentId = 1;
            this.dbContext.MonumentComments.Add(new MonumentComment { Id = commentId, IsDeleted = true });
            this.dbContext.SaveChanges();

            Action testCode = () => this.monumentCommentsService.GetById(commentId);
            testCode.ShouldThrow<CommentDeletedException>();
        }

        [Fact]
        public void GetAllForMonumentOrderedByDateDescending_ReturnOnlyCommentsForGivenMonument()
        {
            int monumentId = 2;
            int commentId = 4;
            this.dbContext.Monuments.Add(new Monument { Id = 1 });
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.MonumentComments.Add(new MonumentComment { Id = 3, MonumentId = 1 });
            this.dbContext.MonumentComments.Add(new MonumentComment { Id = commentId, MonumentId = monumentId });
            this.dbContext.SaveChanges();

            IQueryable<MonumentComment> result = this.monumentCommentsService
                .GetAllForMonumentOrderedByDateDescending(monumentId);
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(commentId);
        }

        [Fact]
        public void GetAllForMonumentOrderedByDateDescending_ReturnOnlyNotDeletedComments()
        {
            int monumentId = 1;
            int commentId = 3;
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.MonumentComments.Add(new MonumentComment { Id = 2, MonumentId = monumentId, IsDeleted = true });
            this.dbContext.MonumentComments.Add(new MonumentComment { Id = commentId, MonumentId = monumentId });
            this.dbContext.SaveChanges();

            IQueryable<MonumentComment> result = this.monumentCommentsService
                .GetAllForMonumentOrderedByDateDescending(monumentId);
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(commentId);
        }

        [Fact]
        public void GetAllForMonumentOrderedByDateDescending_ReturnCommentsOrderedByDateDescending()
        {
            int monumentId = 1;
            int commentId = 3;
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.MonumentComments.Add(new MonumentComment
            { Id = 2, MonumentId = monumentId, CreatedOn = new DateTime() });
            this.dbContext.MonumentComments.Add(new MonumentComment
            { Id = commentId, MonumentId = monumentId, CreatedOn = DateTime.UtcNow });
            this.dbContext.SaveChanges();

            IQueryable<MonumentComment> result = this.monumentCommentsService
                .GetAllForMonumentOrderedByDateDescending(monumentId);
            result.Count().ShouldBe(2);
            result.First().Id.ShouldBe(commentId);
        }

        [Fact]
        public void GetAllForMonumentOrderedByDateDescending_DbContextWithoutComments_ReturnsEmptyCollection()
        {
            int result = this.monumentCommentsService.GetAllForMonumentOrderedByDateDescending(1).Count();
            result.ShouldBe(0);
        }

        [Fact]
        public void Create_ShouldCreateNewCommentCorrectly()
        {
            int monumentId = 1;
            string username = "testUsername";
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.Users.Add(new MbUser { UserName = username });
            this.dbContext.SaveChanges();

            string content = "testContent";
            this.monumentCommentsService.Create(monumentId, content, username);
            MonumentComment result = this.dbContext.MonumentComments.First();

            result.ShouldSatisfyAllConditions
            (
                () => result.MonumentId.ShouldBe(monumentId),
                () => result.Content.ShouldBe(content),
                () => result.User.UserName.ShouldBe(username)
            );
        }

        [Fact]
        public void Create_ThrowExceptionForNullContent()
        {
            int monumentId = 1;
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.SaveChanges();

            Action testCode = () => this.monumentCommentsService.Create(monumentId, null, "");
            testCode.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Create_ThrowExceptionForEmptyContent()
        {
            int monumentId = 1;
            this.dbContext.Monuments.Add(new Monument { Id = monumentId });
            this.dbContext.SaveChanges();

            Action testCode = () => this.monumentCommentsService.Create(monumentId, "   ", "");
            testCode.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Like_ShouldCreateNewLikeCorrectly()
        {

            int commentId = 1;
            string username = "testUsername";
            this.dbContext.MonumentComments.Add(new MonumentComment { Id = commentId });
            this.dbContext.Users.Add(new MbUser { UserName = username });
            this.dbContext.SaveChanges();

            this.monumentCommentsService.Like(commentId, username);
            MonumentCommentLike result = this.dbContext.MonumentCommentLikes.First();

            result.ShouldSatisfyAllConditions
            (
                () => result.MonumentCommentId.ShouldBe(commentId),
                () => result.User.UserName.ShouldBe(username)
            );
        }

        [Fact]
        public void Dislike_RemoveLikeCorrectly()
        {
            int commentId = 1;
            string username = "testUsername";
            this.dbContext.MonumentCommentLikes.Add(new MonumentCommentLike
            {
                MonumentComment = new MonumentComment { Id = commentId },
                User = new MbUser { UserName = username },
            });
            this.dbContext.SaveChanges();

            this.monumentCommentsService.Dislike(commentId, username);
            int result = this.dbContext.MonumentCommentLikes.Count();

            result.ShouldBe(0);
        }

        [Fact]
        public void Dislike_ThrowExceptionForNotExistingLike()
        {
            int commentId = 1;
            string username = "testUsername";
            this.dbContext.MonumentComments.Add(new MonumentComment { Id = commentId });
            this.dbContext.Users.Add(new MbUser { UserName = username });
            this.dbContext.SaveChanges();

            Action testCode = () => this.monumentCommentsService.Dislike(commentId, username);
            testCode.ShouldThrow<LikeNullException>();
        }

        [Fact]
        public void CheckForExistingLike_ReturnTrueIfLikeExist()
        {
            int commentId = 1;
            string username = "testUsername";
            this.dbContext.MonumentCommentLikes.Add(new MonumentCommentLike
            {
                MonumentComment = new MonumentComment { Id = commentId },
                User = new MbUser { UserName = username },
            });
            this.dbContext.SaveChanges();

            bool result = this.monumentCommentsService.CheckForExistingLike(commentId, username);
            result.ShouldBe(true);
        }

        [Fact]
        public void CheckForExistingLike_ReturnFalseIfLikeDoNotExist()
        {
            int commentId = 1;
            string username = "testUsername";
            this.dbContext.MonumentComments.Add(new MonumentComment { Id = commentId });
            this.dbContext.Users.Add(new MbUser { UserName = username });
            this.dbContext.SaveChanges();

            bool result = monumentCommentsService.CheckForExistingLike(commentId, username);
            result.ShouldBe(false);
        }

        [Fact]
        public void Delete_SetCommentIsDeleteToTrue()
        {
            int commentId = 1;
            this.dbContext.MonumentComments.Add(new MonumentComment { Id = commentId });
            this.dbContext.SaveChanges();

            this.monumentCommentsService.Delete(commentId);

            bool result = this.dbContext.MonumentComments.First().IsDeleted;
            result.ShouldBe(true);
        }
    }
}
