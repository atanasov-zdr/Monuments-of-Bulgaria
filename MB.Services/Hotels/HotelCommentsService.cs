namespace MB.Services.Hotels
{
    using System;
    using System.Linq;

    using Common.Exceptions;
    using Contracts.Hotels;
    using Contracts.Users;
    using Data;
    using Models;
    using Models.Hotels;

    public class HotelCommentsService : IHotelCommentsService
    {
        private readonly MbDbContext dbContext;
        private readonly IUsersService usersService;
        private readonly IHotelsService hotelsService;

        public HotelCommentsService(MbDbContext dbContext, IUsersService usersService, IHotelsService hotelsService)
        {
            this.dbContext = dbContext;
            this.usersService = usersService;
            this.hotelsService = hotelsService;
        }

        public HotelComment GetById(int commentId)
        {
            HotelComment comment = this.dbContext.HotelComments.FirstOrDefault(x => x.Id == commentId);

            if (comment == null)
                throw new CommentNullException();

            if (comment.IsDeleted == true)
                throw new CommentDeletedException();

            return comment;
        }

        public IQueryable<HotelComment> GetAllForHotelOrderedByDateDescending(int hotelId)
        {
            return this.dbContext.HotelComments
                .Where(x => x.HotelId == hotelId)
                .Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.CreatedOn);
        }

        public void Create(int hotelId, string content, string username)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException(nameof(content));

            MbUser user = this.usersService.GetByUsername(username);
            Hotel hotel = this.hotelsService.GetById(hotelId);

            var hotelComment = new HotelComment
            {
                Content = content,
                Hotel = hotel,
                User = user,
            };

            this.dbContext.HotelComments.Add(hotelComment);
            this.dbContext.SaveChanges();
        }

        public void Like(int commentId, string username)
        {
            HotelComment comment = this.GetById(commentId);
            MbUser user = this.usersService.GetByUsername(username);

            var like = new HotelCommentLike
            {
                HotelComment = comment,
                User = user,
            };

            this.dbContext.HotelCommentLikes.Add(like);
            this.dbContext.SaveChanges();
        }

        public void Dislike(int commentId, string username)
        {
            HotelComment comment = this.GetById(commentId);
            MbUser user = this.usersService.GetByUsername(username);

            var like = this.dbContext.HotelCommentLikes.SingleOrDefault(x => x.HotelComment == comment && x.User == user);
            if (like == null)
                throw new LikeNullException();

            this.dbContext.HotelCommentLikes.Remove(like);
            this.dbContext.SaveChanges();
        }

        public bool CheckForExistingLike(int commentId, string username)
        {
            HotelComment comment = this.GetById(commentId);
            MbUser user = this.usersService.GetByUsername(username);

            bool result = this.dbContext.HotelCommentLikes.Any(x => x.HotelComment == comment && x.User == user);
            return result;
        }

        public void Delete(int commentId)
        {
            HotelComment comment = this.GetById(commentId);
            comment.IsDeleted = true;
            this.dbContext.SaveChanges();
        }
    }
}
