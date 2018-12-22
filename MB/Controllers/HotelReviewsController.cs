namespace MB.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Base;
    using ViewModels.Hotels.HotelReviews;
    using Services.Contracts.Hotels;

    public class HotelReviewsController : BaseController
    {
        private readonly IHotelReviewsService hotelReviewsService;

        public HotelReviewsController(IHotelReviewsService hotelReviewsService)
        {
            this.hotelReviewsService = hotelReviewsService;
        }

        [Authorize]
        public IActionResult Write(int hotelId)
        {
            string hotelName = this.hotelReviewsService.GetNameById(hotelId);
            var viewModel = new HotelReviewWriteViewModel { HotelId = hotelId, HotelName = hotelName };
            return base.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Write(HotelReviewWriteViewModel model)
        {
            if (!ModelState.IsValid)
                return base.RedirectToAction("Write", "HotelReviews", new { model.HotelId });

            this.hotelReviewsService.Create(model, this.User.Identity.Name);

            return base.RedirectToAction("Details", "Hotels", new { model.HotelId });
        }
    }
}
