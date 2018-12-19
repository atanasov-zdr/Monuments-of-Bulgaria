namespace MB.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class MonumentCommentsController : Controller
    {
        public IActionResult Write(int monumentId, string content)
        {
            return base.View();
        }
    }
}