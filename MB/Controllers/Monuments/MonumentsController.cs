namespace MB.Controllers.Monuments
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using ReflectionIT.Mvc.Paging;

    using Common;
    using Mapping;
    using Models.Monuments;
    using Services.Contracts.Monuments;
    using Services.Contracts.Oblasts;
    using ViewModels.Monuments;
    using ViewModels.Monuments.MonumentComments;
    using ViewModels.Monuments.MonumentReviews;

    public class MonumentsController : Controller
    {
        private readonly IMonumentsService monumentsService;
        private readonly IMonumentCommentsService monumentCommentsService;
        private readonly IOblastsService oblastsService;
        private readonly IMapper mapper;
        private const int PageSize = 12;

        public MonumentsController(
            IMonumentsService monumentsService, 
            IMonumentCommentsService monumentCommentsService,
            IOblastsService oblastsService,
            IMapper mapper)
        {
            this.monumentsService = monumentsService;
            this.monumentCommentsService = monumentCommentsService;
            this.oblastsService = oblastsService;
            this.mapper = mapper;
        }

        public IActionResult All(int page = 1)
        {
            IEnumerable<MonumentAllViewModel> monuments = this.monumentsService
                .GetAllOrderedByName()
                .To<MonumentAllViewModel>()
                .ToList();
            
            IPagingList<MonumentAllViewModel> viewModel = PagingList.Create(monuments, PageSize, page);
            return View(viewModel);         
        }

        public IActionResult AllForOblast(int oblastId, int page = 1)
        {
            IEnumerable<MonumentAllViewModel> monuments = this.monumentsService
                .GetAllForOblastOrderedByName(oblastId)
                .To<MonumentAllViewModel>()
                .ToList();
            
            IPagingList<MonumentAllViewModel> viewModel = PagingList.Create(monuments, PageSize, page);
            return base.View(viewModel);
        }        

        public IActionResult Details(int monumentId)
        {
            Monument monument = this.monumentsService.GetById(monumentId);

            var viewModel = this.mapper.Map<MonumentDetailsViewModel>(monument);

            var reviews = this.mapper.Map<MonumentReviewsViewModel>(monument);
            viewModel.Reviews = reviews;

            var comments = this.monumentCommentsService.GetAllForMonumentOrderedByDateDescending(monumentId)
                .To<MonumentCommentViewModel>()
                .ToList();
            viewModel.Comments = comments;

            return base.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        public IActionResult Add()
        {
            var oblasts = this.oblastsService.GetAllOrderedByName()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
            var viewModel = new MonumentAddViewModel { Oblasts = oblasts };

            return base.View(viewModel);            
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        public IActionResult Add(MonumentAddViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var oblasts = this.oblastsService.GetAllOrderedByName()
                   .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                   .ToList();
                model.Oblasts = oblasts;

                return base.View(model);
            }

            int monumentId = this.monumentsService.Add(model);

            return base.RedirectToAction("Details", new { monumentId });
        }
    }
}
