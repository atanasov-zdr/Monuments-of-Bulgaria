namespace MB.Controllers.Hotels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using ReflectionIT.Mvc.Paging;
    
    using Mapping;
    using Models.Hotels;
    using Services.Contracts.Hotels;
    using ViewModels.Hotels;
    using ViewModels.Hotels.HotelComments;
    using ViewModels.Hotels.HotelReviews;
    
    public class HotelsController : Controller
    {
        private readonly IHotelsService hotelsService;
        private readonly IHotelCommentsService hotelCommentsService;
        private readonly IMapper mapper;

        public HotelsController(IHotelsService hotelsService, IHotelCommentsService hotelCommentsService, IMapper mapper)
        {
            this.hotelsService = hotelsService;
            this.hotelCommentsService = hotelCommentsService;
            this.mapper = mapper;
        }

        public IActionResult All(int page = 1)
        {
            IEnumerable<HotelAllViewModel> hotels = this.hotelsService
                .GetAllOrderedByName()
                .To<HotelAllViewModel>()
                .ToList();

            int pageSize = 12;
            IPagingList<HotelAllViewModel> viewModel = PagingList.Create(hotels, pageSize, page);
            return View(viewModel);
        }

        public IActionResult AllForOblast(int oblastId, int page = 1)
        {
            IEnumerable<HotelAllViewModel> hotels = this.hotelsService
                .GetAllForOblastOrderedByName(oblastId)
                .To<HotelAllViewModel>()
                .ToList();

            int pageSize = 12;
            IPagingList<HotelAllViewModel> viewModel = PagingList.Create(hotels, pageSize, page);
            return base.View(viewModel);
        }

        public IActionResult Details(int hotelId)
        {
            Hotel hotel = this.hotelsService.GetById(hotelId);

            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            var viewModel = this.mapper.Map<HotelDetailsViewModel>(hotel);

            var reviews = this.mapper.Map<HotelReviewsViewModel>(hotel);
            viewModel.Reviews = reviews;

            var comments = this.hotelCommentsService.GetAllForHotelOrderedByDateDescending(hotelId)
                .To<HotelCommentViewModel>()
                .ToList();
            viewModel.Comments = comments;

            return base.View(viewModel);
        }
    }
}
