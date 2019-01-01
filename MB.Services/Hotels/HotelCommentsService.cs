namespace MB.Services.Hotels
{
    using System;
    using System.Linq;

    using Contracts.Hotels;
    using Data;
    using Models;
    using Models.Hotels;

    public class HotelCommentsService : IHotelCommentsService
    {
        private readonly MbDbContext dbContext;

        public HotelCommentsService(MbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public HotelComment GetById(int commentId)
        {
            HotelComment comment = this.dbContext.HotelComments.FirstOrDefault(x => x.Id == commentId);

            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            if (comment.IsDeleted == true)
                throw new ArgumentNullException(nameof(comment));

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
            if (!this.dbContext.Hotels.Any(x => x.Id == hotelId))
                throw new ArgumentNullException(nameof(hotelId));

            MbUser user = this.dbContext.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var hotelComment = new HotelComment
            {
                Content = content,
                HotelId = hotelId,
                UserId = user.Id,
            };

            this.dbContext.HotelComments.Add(hotelComment);
            this.dbContext.SaveChanges();
        }

        public void Like(int commentId, string username)
        {
            HotelComment comment = this.GetById(commentId);

            MbUser user = this.dbContext.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var like = new HotelCommentLike
            {
                HotelCommentId = commentId,
                UserId = user.Id,
            };

            this.dbContext.HotelCommentLikes.Add(like);
            this.dbContext.SaveChanges();
        }

        public void Dislike(int commentId, string username)
        {
            HotelComment comment = this.GetById(commentId);

            MbUser user = this.dbContext.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var like = this.dbContext.HotelCommentLikes.SingleOrDefault(x => x.HotelComment == comment && x.User == user);
            if (like == null)
                throw new ArgumentNullException(nameof(like));

            this.dbContext.HotelCommentLikes.Remove(like);
            this.dbContext.SaveChanges();
        }

        public bool CheckForExistingLike(int commentId, string username)
        {
            MbUser user = this.dbContext.Users.FirstOrDefault(x => x.UserName == username);

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            bool result = this.dbContext.HotelCommentLikes.Any(x => x.HotelCommentId == commentId && x.User == user);
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
