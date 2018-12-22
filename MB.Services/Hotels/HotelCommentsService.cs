namespace MB.Services.Hotels
{
    using System;
    using System.Linq;

    using Contracts.Hotels;
    using Data;
    using Models;

    public class HotelCommentsService : IHotelCommentsService
    {
        private readonly MbDbContext dbContext;

        public HotelCommentsService(MbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<HotelComment> GetAllForHotel(int hotelId)
        {
            return this.dbContext.HotelComments.Where(x => x.HotelId == hotelId);
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
            HotelComment comment = this.dbContext.HotelComments.FirstOrDefault(x => x.Id == commentId);
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));
            
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
    }
}
