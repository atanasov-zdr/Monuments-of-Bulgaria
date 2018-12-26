﻿namespace MB.Controllers.Monuments
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Common;
    using Services.Contracts.Monuments;
    
    public class MonumentCommentsController : Controller
    {
        private readonly IMonumentCommentsService monumentCommentsService;

        public MonumentCommentsController(IMonumentCommentsService monumentCommentsService)
        {
            this.monumentCommentsService = monumentCommentsService;
        }

        [HttpPost]
        [Authorize]
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

        [Authorize]
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
    }
}