namespace MB.Services
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Contracts;
    using Data;
    using Models;
    using ViewModels.MonumentReviews;

    public class MonumentReviewsService : IMonumentReviewsService
    {
        private readonly MbDbContext dbContext;
        private readonly IMapper mapper;

        public MonumentReviewsService(MbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public string GetNameById(int monumentId)
        {
            Monument monument = this.dbContext.Monuments.FirstOrDefault(x => x.Id == monumentId);

            if (monument == null)
                throw new ArgumentNullException(nameof(monument));

            if (monument.Name == null)
                throw new ArgumentNullException(nameof(monument.Name));

            return monument.Name;
        }

        public void Create(MonumentReviewWriteViewModel model, string username)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            
            if (username == null)
                throw new ArgumentNullException(nameof(username));

            MbUser user = this.dbContext.Users.FirstOrDefault(x => x.UserName == username);

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            MonumentReview monumentReview = this.mapper.Map<MonumentReview>(model);
            monumentReview.UserId = user.Id;

            this.dbContext.MonumentReviews.Add(monumentReview);
            this.dbContext.SaveChanges();
        }
    }
}
