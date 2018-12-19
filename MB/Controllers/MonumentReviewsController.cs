namespace MB.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class MonumentReviewsController : Controller
    {
        public IActionResult Write(int monumentId)
        {
            return base.View();
        }
    }
}