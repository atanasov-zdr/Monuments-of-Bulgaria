namespace MB.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Base;
    using Services.Contracts.Monuments;

    public class MonumentCommentsController : BaseController
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
                return base.RedirectToAction("Details", "Monuments", new { monumentId });

            this.monumentCommentsService.Create(monumentId, content, this.User.Identity.Name);
            
            return base.RedirectToAction("Details", "Monuments", new { monumentId });
        }

        [Authorize]
        public IActionResult Like(int monumentId, int commentId)
        {
            this.monumentCommentsService.Like(commentId, this.User.Identity.Name);
            return base.RedirectToAction("Details", "Monuments", new { monumentId });
        }
    }
}