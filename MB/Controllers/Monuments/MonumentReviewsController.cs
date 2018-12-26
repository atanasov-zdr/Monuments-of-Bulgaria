namespace MB.Controllers.Monuments
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    using ViewModels.Monuments.MonumentReviews;
    using Services.Contracts.Monuments;
    
    public class MonumentReviewsController : Controller
    {
        private readonly IMonumentReviewsService monumentReviewsService;

        public MonumentReviewsController(IMonumentReviewsService monumentReviewsService)
        {
            this.monumentReviewsService = monumentReviewsService;
        }

        [Authorize]
        public IActionResult Write(int monumentId)
        {
            //if (this.monumentReviewsService.CheckForExistingReview(monumentId, this.User.Identity.Name))
            //    return this.RedirectToAction("Details", "Monuments", new { monumentId });

            string monumentName = this.monumentReviewsService.GetNameById(monumentId);
            var viewModel = new MonumentReviewWriteViewModel { MonumentId = monumentId, MonumentName = monumentName };
            return base.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Write(MonumentReviewWriteViewModel model)
        {
            if (!ModelState.IsValid)
                return base.RedirectToAction("Write", "MonumentReviews", new { model.MonumentId });

            this.monumentReviewsService.Create(model, this.User.Identity.Name);

            return base.RedirectToAction("Details", "Monuments", new { model.MonumentId });
        }
    }
}