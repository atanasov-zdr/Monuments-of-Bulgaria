namespace MB.Services.Monuments
{
    using System.Linq;

    using AutoMapper;

    using Common.Exceptions;
    using Contracts.Monuments;
    using Contracts.Users;
    using Data;
    using Models;
    using Models.Monuments;
    using ViewModels.Monuments.MonumentReviews;

    public class MonumentReviewsService : IMonumentReviewsService
    {
        private readonly MbDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IUsersService usersService;
        private readonly IMonumentsService monumentsService;

        public MonumentReviewsService(MbDbContext dbContext,
            IMapper mapper,
            IUsersService usersService,
            IMonumentsService monumentsService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.usersService = usersService;
            this.monumentsService = monumentsService;
        }

        public void Create(MonumentReviewWriteViewModel model, string username)
        {
            if (!this.monumentsService.CheckExistById(model.MonumentId))
                throw new MonumentNullException();
            
            MonumentReview monumentReview = this.mapper.Map<MonumentReview>(model);

            MbUser user = this.usersService.GetByUsername(username);
            monumentReview.User = user;

            this.dbContext.MonumentReviews.Add(monumentReview);
            this.dbContext.SaveChanges();
        }

        public bool CheckForExistingReview(int monumentId, string username)
        {
            Monument monument = this.monumentsService.GetById(monumentId);
            MbUser user = this.usersService.GetByUsername(username);

            bool result = this.dbContext.MonumentReviews.Any(x => x.Monument == monument && x.User == user);
            return result;
        }
    }
}
