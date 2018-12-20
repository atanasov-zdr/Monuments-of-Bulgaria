namespace MB.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;

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
                return base.RedirectToAction("Details", "Monuments", new { monumentId });

            this.monumentCommentsService.Create(monumentId, content, this.User.Identity.Name);
            
            return base.RedirectToAction("Details", "Monuments", new { monumentId });
        }

        public IActionResult Like(int monumentId, int commentId)
        {
            this.monumentCommentsService.Like(commentId);
            return base.RedirectToAction("Details", "Monuments", new { monumentId });
        }

        public IActionResult Dislike(int monumentId, int commentId)
        {
            this.monumentCommentsService.Dislike(commentId);
            return base.RedirectToAction("Details", "Monuments", new { monumentId });
        }
    }
}