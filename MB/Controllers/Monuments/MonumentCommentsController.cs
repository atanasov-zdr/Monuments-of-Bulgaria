namespace MB.Controllers.Monuments
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Common;
    using Services.Contracts.Monuments;
    
    [Authorize]
    public class MonumentCommentsController : Controller
    {
        private readonly IMonumentCommentsService monumentCommentsService;

        public MonumentCommentsController(IMonumentCommentsService monumentCommentsService)
        {
            this.monumentCommentsService = monumentCommentsService;
        }

        [HttpPost]
        public IActionResult Write(int monumentId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                string errorMsg = "Comment cannot be empty!";
                return base.View(GlobalConstants.ErrorViewName, errorMsg);
            }

            this.monumentCommentsService.Create(monumentId, content, this.User.Identity.Name); 
            return base.RedirectToAction("Details", "Monuments", new { monumentId });
        }
        
        public IActionResult Like(int monumentId, int commentId)
        {
            if (this.monumentCommentsService.CheckForExistingLike(commentId, this.User.Identity.Name))
            {
                string errorMsg = "You already liked this comment!";
                return base.View(GlobalConstants.ErrorViewName, errorMsg);
            }

            this.monumentCommentsService.Like(commentId, this.User.Identity.Name);
            return base.RedirectToAction("Details", "Monuments", new { monumentId });
        }

        public IActionResult Dislike(int monumentId, int commentId)
        {
            if (!this.monumentCommentsService.CheckForExistingLike(commentId, this.User.Identity.Name))
            {
                string errorMsg = "You are not liked this comment!";
                return base.View(GlobalConstants.ErrorViewName, errorMsg);
            }

            this.monumentCommentsService.Dislike(commentId, this.User.Identity.Name);
            return base.RedirectToAction("Details", "Monuments", new { monumentId });
        }

        [HttpPost]
        public IActionResult Delete(int commentId, int monumentId)
        {
            this.monumentCommentsService.Delete(commentId);
            return base.RedirectToAction("Details", "Monuments", new { monumentId });
        }
    }
}