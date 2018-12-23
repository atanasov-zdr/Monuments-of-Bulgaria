namespace MB.Controllers.Hotels
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    using Services.Contracts.Hotels;
    
    public class HotelCommentsController : Controller
    {
        private readonly IHotelCommentsService hotelCommentsService;

        public HotelCommentsController(IHotelCommentsService hotelCommentsService)
        {
            this.hotelCommentsService = hotelCommentsService;
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult Write(int hotelId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return base.RedirectToAction("Details", "Hotels", new { hotelId });

            this.hotelCommentsService.Create(hotelId, content, this.User.Identity.Name);

            return base.RedirectToAction("Details", "Hotels", new { hotelId });
        }

        [Authorize]
        public IActionResult Like(int hotelId, int commentId)
        {
            this.hotelCommentsService.Like(commentId, this.User.Identity.Name);
            return base.RedirectToAction("Details", "Hotels", new { hotelId });
        }
    }
}
