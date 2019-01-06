namespace MB.Controllers.Hotels
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
    using Models.Hotels;
    using Services.Contracts.Hotels;
    using Services.Contracts.Oblasts;
    using ViewModels.Hotels;
    using ViewModels.Hotels.HotelComments;
    using ViewModels.Hotels.HotelReviews;

    public class HotelsController : Controller
    {
        private readonly IHotelsService hotelsService;
        private readonly IHotelReviewsService hotelReviewsService;
        private readonly IHotelCommentsService hotelCommentsService;
        private readonly IOblastsService oblastsService;
        private readonly IMapper mapper;
        private const int PageSize = 12;

        public HotelsController(
            IHotelsService hotelsService,
            IHotelReviewsService hotelReviewsService,
            IHotelCommentsService hotelCommentsService,
            IOblastsService oblastsService,
            IMapper mapper)
        {
            this.hotelsService = hotelsService;
            this.hotelReviewsService = hotelReviewsService;
            this.hotelCommentsService = hotelCommentsService;
            this.oblastsService = oblastsService;
            this.mapper = mapper;
        }

        public IActionResult All(int page = 1)
        {
            IEnumerable<HotelAllViewModel> hotels = this.hotelsService
                .GetAllOrderedByName()
                .To<HotelAllViewModel>()
                .ToList();
            
            IPagingList<HotelAllViewModel> viewModel = PagingList.Create(hotels, PageSize, page);
            return View(viewModel);
        }

        public IActionResult AllForOblast(int oblastId, int page = 1)
        {
            IEnumerable<HotelAllViewModel> hotels = this.hotelsService
                .GetAllForOblastOrderedByName(oblastId)
                .To<HotelAllViewModel>()
                .ToList();
            
            IPagingList<HotelAllViewModel> viewModel = PagingList.Create(hotels, PageSize, page);
            return base.View(viewModel);
        }

        public IActionResult Details(int hotelId)
        {
            Hotel hotel = this.hotelsService.GetById(hotelId); 
            var viewModel = this.mapper.Map<HotelDetailsViewModel>(hotel);
            viewModel.HasReview = this.hotelReviewsService.CheckForExistingReview(hotel.Id, this.User.Identity.Name);

            var reviews = this.mapper.Map<HotelReviewsViewModel>(hotel);
            viewModel.Reviews = reviews;

            List<HotelComment> dbComments = this.hotelCommentsService.GetAllForHotelOrderedByDateDescending(hotelId).ToList();
            var comments = new List<HotelCommentViewModel>();
            foreach (HotelComment comment in dbComments)
            {
                var commentModel = this.mapper.Map<HotelCommentViewModel>(comment);
                commentModel.IsLiked = comment.Likes.Any(x => x.User.UserName == this.User.Identity.Name);
                comments.Add(commentModel);
            }
            viewModel.Comments = comments;

            return base.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        public IActionResult Add()
        {
            var oblasts = this.oblastsService.GetAllOrderedByName()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
            var viewModel = new HotelAddViewModel { Oblasts = oblasts };

            return base.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        public IActionResult Add(HotelAddViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var oblasts = this.oblastsService.GetAllOrderedByName()
                   .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                   .ToList();
                model.Oblasts = oblasts;

                return base.View(model);
            }

            int hotelId = this.hotelsService.Add(model);

            return base.RedirectToAction("Details", new { hotelId } );
        }

        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        public IActionResult Edit(int hotelId)
        {
            Hotel hotel = this.hotelsService.GetById(hotelId);
            var viewModel = this.mapper.Map<HotelEditViewModel>(hotel);

            var oblasts = this.oblastsService.GetAllOrderedByName()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
            viewModel.Oblasts = oblasts;

            return base.View(viewModel);           
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        public IActionResult Edit(HotelEditViewModel model)
        {
            if (!this.ModelState.IsValid)
                return base.RedirectToAction("Edit", new { hotelId = model.Id });
            
            this.hotelsService.Update(model);

            return base.RedirectToAction("Details", new { hotelId = model.Id });
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        public IActionResult Delete(int hotelId)
        {
            this.hotelsService.Delete(hotelId);
            return base.RedirectToAction("All");
        }
    }
}
