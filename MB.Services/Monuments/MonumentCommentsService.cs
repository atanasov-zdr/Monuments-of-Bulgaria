namespace MB.Services.Monuments
{
    using System;
    using System.Linq;
    
    using Contracts.Monuments;
    using Data;
    using Models;
    using Models.Monuments;

    public class MonumentCommentsService : IMonumentCommentsService
    {
        private readonly MbDbContext dbContext;

        public MonumentCommentsService(MbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public MonumentComment GetById(int commentId)
        {
            MonumentComment comment = this.dbContext.MonumentComments.FirstOrDefault(x => x.Id == commentId);

            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            if (comment.IsDeleted == true)
                throw new ArgumentNullException(nameof(comment));

            return comment;
        }

        public IQueryable<MonumentComment> GetAllForMonumentOrderedByDateDescending(int monumentId)
        {
            return this.dbContext.MonumentComments
                .Where(x => x.MonumentId == monumentId)
                .Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.CreatedOn);
        }

        public void Create(int monumentId, string content, string username)
        {
            if (!this.dbContext.Monuments.Any(x => x.Id == monumentId))
                throw new ArgumentNullException(nameof(monumentId));
            
            MbUser user = this.dbContext.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var monumentComment = new MonumentComment
            {
                Content = content,
                MonumentId = monumentId,
                UserId = user.Id,
            };

            this.dbContext.MonumentComments.Add(monumentComment);
            this.dbContext.SaveChanges();
        }

        public void Like(int commentId, string username)
        {
            MonumentComment comment = this.GetById(commentId);

            MbUser user = this.dbContext.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var like = new MonumentCommentLike
            {
                MonumentCommentId = commentId,
                UserId = user.Id,
            };

            this.dbContext.MonumentCommentLikes.Add(like);
            this.dbContext.SaveChanges();
        }

        public void Dislike(int commentId, string username)
        {
            MonumentComment comment = this.GetById(commentId);

            MbUser user = this.dbContext.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var like = this.dbContext.MonumentCommentLikes.SingleOrDefault(x => x.MonumentComment == comment && x.User == user);
            if (like == null)
                throw new ArgumentNullException(nameof(like));

            this.dbContext.MonumentCommentLikes.Remove(like);
            this.dbContext.SaveChanges();
        }

        public bool CheckForExistingLike(int commentId, string username)
        {
            MbUser user = this.dbContext.Users.FirstOrDefault(x => x.UserName == username);

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            bool result = this.dbContext.MonumentCommentLikes.Any(x => x.MonumentCommentId == commentId && x.User == user);
            return result;
        }

        public void Delete(int commentId)
        {
            MonumentComment comment = this.GetById(commentId);
            comment.IsDeleted = true;
            this.dbContext.SaveChanges();
        }
    }
}
