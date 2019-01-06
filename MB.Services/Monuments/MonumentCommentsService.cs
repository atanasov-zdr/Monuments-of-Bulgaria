namespace MB.Services.Monuments
{
    using System;
    using System.Linq;

    using Common.Exceptions;
    using Contracts.Monuments;
    using Contracts.Users;
    using Data;
    using Models;
    using Models.Monuments;

    public class MonumentCommentsService : IMonumentCommentsService
    {
        private readonly MbDbContext dbContext;
        private readonly IMonumentsService monumentsService;
        private readonly IUsersService usersService;

        public MonumentCommentsService(MbDbContext dbContext, IMonumentsService monumentsService, IUsersService usersService)
        {
            this.dbContext = dbContext;
            this.monumentsService = monumentsService;
            this.usersService = usersService;
        }

        public MonumentComment GetById(int commentId)
        {
            MonumentComment comment = this.dbContext.MonumentComments.FirstOrDefault(x => x.Id == commentId);

            if (comment == null)
                throw new CommentNullException();

            if (comment.IsDeleted == true)
                throw new CommentDeletedException();

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
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException(nameof(content));

            MbUser user = this.usersService.GetByUsername(username);
            Monument monument = this.monumentsService.GetById(monumentId);

            var monumentComment = new MonumentComment
            {
                Content = content,
                Monument = monument,
                User = user,
            };

            this.dbContext.MonumentComments.Add(monumentComment);
            this.dbContext.SaveChanges();
        }

        public void Like(int commentId, string username)
        {
            MonumentComment comment = this.GetById(commentId);
            MbUser user = this.usersService.GetByUsername(username);

            var like = new MonumentCommentLike
            {
                MonumentComment = comment,
                User = user,
            };

            this.dbContext.MonumentCommentLikes.Add(like);
            this.dbContext.SaveChanges();
        }

        public void Dislike(int commentId, string username)
        {
            MonumentComment comment = this.GetById(commentId);
            MbUser user = this.usersService.GetByUsername(username);

            var like = this.dbContext.MonumentCommentLikes.SingleOrDefault(x => x.MonumentComment == comment && x.User == user);
            if (like == null)
                throw new LikeNullException();

            this.dbContext.MonumentCommentLikes.Remove(like);
            this.dbContext.SaveChanges();
        }

        public bool CheckForExistingLike(int commentId, string username)
        {
            MonumentComment comment = this.GetById(commentId);
            MbUser user = this.usersService.GetByUsername(username);

            bool result = this.dbContext.MonumentCommentLikes.Any(x => x.MonumentComment == comment && x.User == user);
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
