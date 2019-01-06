namespace MB.Controllers.Monuments
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Common;
    using ViewModels.Monuments.MonumentReviews;
    using Services.Contracts.Monuments;
    
    [Authorize]
    public class MonumentReviewsController : Controller
    {
        private readonly IMonumentReviewsService monumentReviewsService;
        private readonly IMonumentsService monumentsService;

        public MonumentReviewsController(IMonumentReviewsService monumentReviewsService, IMonumentsService monumentsService)
        {
            this.monumentReviewsService = monumentReviewsService;
            this.monumentsService = monumentsService;
        }
        
        public IActionResult Write(int monumentId)
        {
            if (this.monumentReviewsService.CheckForExistingReview(monumentId, this.User.Identity.Name))
            {
                string errorMsg = "You already write a review for this monument!";
                return base.View(GlobalConstants.ErrorViewName, errorMsg);
            }

            string monumentName = this.monumentsService.GetNameById(monumentId);
            var viewModel = new MonumentReviewWriteViewModel { MonumentId = monumentId, MonumentName = monumentName };
            return base.View(viewModel);
        }

        [HttpPost]
        public IActionResult Write(MonumentReviewWriteViewModel model)
        {
            if (!ModelState.IsValid)
                return this.Write(model.MonumentId);

            this.monumentReviewsService.Create(model, this.User.Identity.Name);
            return base.RedirectToAction("Details", "Monuments", new { model.MonumentId });
        }
    }
}