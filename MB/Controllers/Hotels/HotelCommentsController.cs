namespace MB.Controllers.Hotels
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Common;
    using Services.Contracts.Hotels;
    
    [Authorize]
    public class HotelCommentsController : Controller
    {
        private readonly IHotelCommentsService hotelCommentsService;

        public HotelCommentsController(IHotelCommentsService hotelCommentsService)
        {
            this.hotelCommentsService = hotelCommentsService;
        }
        
        [HttpPost]
        public IActionResult Write(int hotelId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                string errorMsg = "Comment cannot be empty!";
                return base.View(GlobalConstants.ErrorViewName, errorMsg);
            }

            this.hotelCommentsService.Create(hotelId, content, this.User.Identity.Name);
            return base.RedirectToAction("Details", "Hotels", new { hotelId });
        }
        
        public IActionResult Like(int hotelId, int commentId)
        {
            if (this.hotelCommentsService.CheckForExistingLike(commentId, this.User.Identity.Name))
            {
                string errorMsg = "You already liked this comment!";
                return base.View(GlobalConstants.ErrorViewName, errorMsg);
            }

            this.hotelCommentsService.Like(commentId, this.User.Identity.Name);
            return base.RedirectToAction("Details", "Hotels", new { hotelId });
        }

        public IActionResult Dislike(int hotelId, int commentId)
        {
            if (!this.hotelCommentsService.CheckForExistingLike(commentId, this.User.Identity.Name))
            {
                string errorMsg = "You are not liked this comment!";
                return base.View(GlobalConstants.ErrorViewName, errorMsg);
            }

            this.hotelCommentsService.Dislike(commentId, this.User.Identity.Name);
            return base.RedirectToAction("Details", "Hotels", new { hotelId });
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdminRoleName)]
        public IActionResult Delete (int commentId, int hotelId)
        {
            this.hotelCommentsService.Delete(commentId);
            return base.RedirectToAction("Details", "Hotels", new { hotelId });
        }
    }
}
