namespace MB.Controllers.Hotels
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Common;
    using ViewModels.Hotels.HotelReviews;
    using Services.Contracts.Hotels;
    
    [Authorize]
    public class HotelReviewsController : Controller
    {
        private readonly IHotelReviewsService hotelReviewsService;
        private readonly IHotelsService hotelsService;

        public HotelReviewsController(IHotelReviewsService hotelReviewsService, IHotelsService hotelsService)
        {
            this.hotelReviewsService = hotelReviewsService;
            this.hotelsService = hotelsService;
        }
        
        public IActionResult Write(int hotelId)
        {
            if (this.hotelReviewsService.CheckForExistingReview(hotelId, this.User.Identity.Name))
            {
                string errorMsg = "You already write a review for this hotel!";
                return base.View(GlobalConstants.ErrorViewName, errorMsg);
            }

            string hotelName = this.hotelsService.GetNameById(hotelId);
            var viewModel = new HotelReviewWriteViewModel { HotelId = hotelId, HotelName = hotelName };
            return base.View(viewModel);
        }

        [HttpPost]
        public IActionResult Write(HotelReviewWriteViewModel model)
        {
            if (!ModelState.IsValid)
                return base.RedirectToAction("Write", new { hotelId = model.HotelId });

            this.hotelReviewsService.Create(model, this.User.Identity.Name);

            return base.RedirectToAction("Details", "Hotels", new { model.HotelId });
        }
    }
}
