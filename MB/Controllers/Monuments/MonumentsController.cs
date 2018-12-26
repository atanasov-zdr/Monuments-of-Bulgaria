namespace MB.Controllers.Monuments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using ReflectionIT.Mvc.Paging;
    
    using Mapping;
    using Models.Monuments;
    using Services.Contracts.Monuments;
    using ViewModels.Monuments;
    using ViewModels.Monuments.MonumentComments;
    using ViewModels.Monuments.MonumentReviews;
    
    public class MonumentsController : Controller
    {
        private readonly IMonumentsService monumentsService;
        private readonly IMonumentCommentsService monumentCommentsService;
        private readonly IMapper mapper;

        public MonumentsController(
            IMonumentsService monumentsService, 
            IMonumentCommentsService monumentCommentsService, 
            IMapper mapper)
        {
            this.monumentsService = monumentsService;
            this.monumentCommentsService = monumentCommentsService;
            this.mapper = mapper;
        }

        public IActionResult All(int page = 1)
        {
            IEnumerable<MonumentAllViewModel> monuments = this.monumentsService
                .GetAllOrderedByName()
                .To<MonumentAllViewModel>()
                .ToList();

            int pageSize = 12;
            IPagingList<MonumentAllViewModel> viewModel = PagingList.Create(monuments, pageSize, page);
            return View(viewModel);         
        }

        public IActionResult AllForOblast(int oblastId, int page = 1)
        {
            IEnumerable<MonumentAllViewModel> monuments = this.monumentsService
                .GetAllForOblastOrderedByName(oblastId)
                .To<MonumentAllViewModel>()
                .ToList();

            int pageSize = 12;
            IPagingList<MonumentAllViewModel> viewModel = PagingList.Create(monuments, pageSize, page);
            return base.View(viewModel);
        }        

        public IActionResult Details(int monumentId)
        {
            Monument monument = this.monumentsService.GetById(monumentId);

            if (monument == null)
                throw new ArgumentNullException(nameof(monument));

            var viewModel = this.mapper.Map<MonumentDetailsViewModel>(monument);

            var reviews = this.mapper.Map<MonumentReviewsViewModel>(monument);
            viewModel.Reviews = reviews;

            var comments = this.monumentCommentsService.GetAllForMonumentOrderedByDateDescending(monumentId)
                .To<MonumentCommentViewModel>()
                .ToList();
            viewModel.Comments = comments;

            return base.View(viewModel);
        }
    }
}
