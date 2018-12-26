namespace MB.Controllers.Hotels
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Common;
    using ViewModels.Hotels.HotelReviews;
    using Services.Contracts.Hotels;
    
    public class HotelReviewsController : Controller
    {
        private readonly IHotelReviewsService hotelReviewsService;

        public HotelReviewsController(IHotelReviewsService hotelReviewsService)
        {
            this.hotelReviewsService = hotelReviewsService;
        }

        [Authorize]
        public IActionResult Write(int hotelId)
        {
            if (this.hotelReviewsService.CheckForExistingReview(hotelId, this.User.Identity.Name))
            {
                string errorMsg = "You already write a review for this hotel!";
                return base.View(GlobalConstants.ErrorViewName, errorMsg);
            }

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
