namespace MB.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Base;

    public class MonumentsController : BaseController
    {
        public IActionResult All()
        {
            return base.View();
        }

        public IActionResult AllForOblast(int oblastId)
        {
            return base.View();
        }
    }
}
