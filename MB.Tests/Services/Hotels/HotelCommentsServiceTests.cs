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
    using Models.Hotels;
    using Profiles;

    [Collection(GlobalConstants.TestClassesCollectionName)]
    public class HotelCommentsServiceTests
    {
        private readonly IServiceProvider provider;
        private readonly MbDbContext dbContext;
        private readonly IHotelCommentsService hotelCommentsService;

        public HotelCommentsServiceTests()
        {
            var services = new ServiceCollection();
            services.AddDbContext<MbDbContext>(opt =>
                opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IHotelCommentsService, HotelCommentsService>();
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
            this.hotelCommentsService = this.provider.GetService<IHotelCommentsService>();
        }

        [Fact]
        public void GetById_ReturnCorrectComment()
        {
            int commentId = 2;
            this.dbContext.HotelComments.Add(new HotelComment { Id = 1 });
            this.dbContext.HotelComments.Add(new HotelComment { Id = commentId });
            this.dbContext.SaveChanges();

            int result = this.hotelCommentsService.GetById(commentId).Id;
            result.ShouldBe(commentId);
        }

        [Fact]
        public void GetById_ThrowExceptionForNotExistingCommentWithGivenId()
        {
            Action testCode = () => this.hotelCommentsService.GetById(2);
            testCode.ShouldThrow<CommentNullException>();
        }

        [Fact]
        public void GetById_ThrowExceptionForDeletedComment()
        {
            int commentId = 1;
            this.dbContext.HotelComments.Add(new HotelComment { Id = commentId, IsDeleted = true });
            this.dbContext.SaveChanges();

            Action testCode = () => this.hotelCommentsService.GetById(commentId);
            testCode.ShouldThrow<CommentDeletedException>();
        }

        [Fact]
        public void GetAllForHotelOrderedByDateDescending_ReturnOnlyCommentsForGivenHotel()
        {
            int hotelId = 2;
            int commentId = 4;
            this.dbContext.Hotels.Add(new Hotel { Id = 1 });
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId });
            this.dbContext.HotelComments.Add(new HotelComment { Id = 3, HotelId = 1 });
            this.dbContext.HotelComments.Add(new HotelComment { Id = commentId, HotelId = hotelId });
            this.dbContext.SaveChanges();

            IQueryable<HotelComment> result = this.hotelCommentsService
                .GetAllForHotelOrderedByDateDescending(hotelId);
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(commentId);
        }

        [Fact]
        public void GetAllForHotelOrderedByDateDescending_ReturnOnlyNotDeletedComments()
        {
            int hotelId = 1;
            int commentId = 3;
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId });
            this.dbContext.HotelComments.Add(new HotelComment { Id = 2, HotelId = hotelId, IsDeleted = true });
            this.dbContext.HotelComments.Add(new HotelComment { Id = commentId, HotelId = hotelId });
            this.dbContext.SaveChanges();

            IQueryable<HotelComment> result = this.hotelCommentsService
                .GetAllForHotelOrderedByDateDescending(hotelId);
            result.Count().ShouldBe(1);
            result.First().Id.ShouldBe(commentId);
        }

        [Fact]
        public void GetAllForHotelOrderedByDateDescending_ReturnCommentsOrderedByDateDescending()
        {
            int hotelId = 1;
            int commentId = 3;
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId });
            this.dbContext.HotelComments.Add(new HotelComment
            { Id = 2, HotelId = hotelId, CreatedOn = new DateTime() });
            this.dbContext.HotelComments.Add(new HotelComment
            { Id = commentId, HotelId = hotelId, CreatedOn = DateTime.UtcNow });
            this.dbContext.SaveChanges();

            IQueryable<HotelComment> result = this.hotelCommentsService
                .GetAllForHotelOrderedByDateDescending(hotelId);
            result.Count().ShouldBe(2);
            result.First().Id.ShouldBe(commentId);
        }

        [Fact]
        public void GetAllForHotelOrderedByDateDescending_DbContextWithoutComments_ReturnsEmptyCollection()
        {
            int result = this.hotelCommentsService.GetAllForHotelOrderedByDateDescending(1).Count();
            result.ShouldBe(0);
        }

        [Fact]
        public void Create_ShouldCreateNewCommentCorrectly()
        {
            int hotelId = 1;
            string username = "testUsername";
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId });
            this.dbContext.Users.Add(new MbUser { UserName = username });
            this.dbContext.SaveChanges();

            string content = "testContent";
            this.hotelCommentsService.Create(hotelId, content, username);
            HotelComment result = this.dbContext.HotelComments.First();

            result.ShouldSatisfyAllConditions
            (
                () => result.HotelId.ShouldBe(hotelId),
                () => result.Content.ShouldBe(content),
                () => result.User.UserName.ShouldBe(username)
            );
        }

        [Fact]
        public void Create_ThrowExceptionForNullContent()
        {
            int hotelId = 1;
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId });
            this.dbContext.SaveChanges();

            Action testCode = () => this.hotelCommentsService.Create(hotelId, null, "");
            testCode.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Create_ThrowExceptionForEmptyContent()
        {
            int hotelId = 1;
            this.dbContext.Hotels.Add(new Hotel { Id = hotelId });
            this.dbContext.SaveChanges();

            Action testCode = () => this.hotelCommentsService.Create(hotelId, "   ", "");
            testCode.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Like_ShouldCreateNewLikeCorrectly()
        {

            int commentId = 1;
            string username = "testUsername";
            this.dbContext.HotelComments.Add(new HotelComment { Id = commentId });
            this.dbContext.Users.Add(new MbUser { UserName = username });
            this.dbContext.SaveChanges();

            this.hotelCommentsService.Like(commentId, username);
            HotelCommentLike result = this.dbContext.HotelCommentLikes.First();

            result.ShouldSatisfyAllConditions
            (
                () => result.HotelCommentId.ShouldBe(commentId),
                () => result.User.UserName.ShouldBe(username)
            );
        }

        [Fact]
        public void Dislike_RemoveLikeCorrectly()
        {
            int commentId = 1;
            string username = "testUsername";
            this.dbContext.HotelCommentLikes.Add(new HotelCommentLike
            {
                HotelComment = new HotelComment { Id = commentId },
                User = new MbUser { UserName = username },
            });
            this.dbContext.SaveChanges();

            this.hotelCommentsService.Dislike(commentId, username);
            int result = this.dbContext.HotelCommentLikes.Count();

            result.ShouldBe(0);
        }

        [Fact]
        public void Dislike_ThrowExceptionForNotExistingLike()
        {
            int commentId = 1;
            string username = "testUsername";
            this.dbContext.HotelComments.Add(new HotelComment { Id = commentId });
            this.dbContext.Users.Add(new MbUser { UserName = username });
            this.dbContext.SaveChanges();

            Action testCode = () => this.hotelCommentsService.Dislike(commentId, username);
            testCode.ShouldThrow<LikeNullException>();
        }

        [Fact]
        public void CheckForExistingLike_ReturnTrueIfLikeExist()
        {
            int commentId = 1;
            string username = "testUsername";
            this.dbContext.HotelCommentLikes.Add(new HotelCommentLike
            {
                HotelComment = new HotelComment { Id = commentId },
                User = new MbUser { UserName = username },
            });
            this.dbContext.SaveChanges();

            bool result = this.hotelCommentsService.CheckForExistingLike(commentId, username);
            result.ShouldBe(true);
        }

        [Fact]
        public void CheckForExistingLike_ReturnFalseIfLikeDoNotExist()
        {
            int commentId = 1;
            string username = "testUsername";
            this.dbContext.HotelComments.Add(new HotelComment { Id = commentId });
            this.dbContext.Users.Add(new MbUser { UserName = username });
            this.dbContext.SaveChanges();

            bool result = hotelCommentsService.CheckForExistingLike(commentId, username);
            result.ShouldBe(false);
        }

        [Fact]
        public void Delete_SetCommentIsDeleteToTrue()
        {
            int commentId = 1;
            this.dbContext.HotelComments.Add(new HotelComment { Id = commentId });
            this.dbContext.SaveChanges();

            this.hotelCommentsService.Delete(commentId);

            bool result = this.dbContext.HotelComments.First().IsDeleted;
            result.ShouldBe(true);
        }
    }
}
