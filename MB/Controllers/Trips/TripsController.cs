namespace MB.Controllers.Trips
{
    using System.Linq;

    using AutoMapper;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using Mapping;
    using Models.Trips;
    using Services.Contracts.Hotels;
    using Services.Contracts.Monuments;
    using Services.Contracts.Trips;
    using ViewModels.Trips;

    [Authorize]
    public class TripsController : Controller
    {
        private readonly IMonumentsService monumentsService;
        private readonly IHotelsService hotelsService;
        private readonly ITripsService tripsService;
        private readonly IMapper mapper;

        public TripsController(IMonumentsService monumentsService, 
            IHotelsService hotelsService, 
            ITripsService tripsService,
            IMapper mapper)
        {
            this.monumentsService = monumentsService;
            this.hotelsService = hotelsService;
            this.tripsService = tripsService;
            this.mapper = mapper;
        }
        
        public IActionResult Create()
        {
            var hotels = this.hotelsService.GetAllOrderedByName()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var monuments = this.monumentsService.GetAllOrderedByName()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var viewModel = new TripCreateViewModel { Hotels = hotels, Monuments = monuments };
            return base.View(viewModel);
        }
        
        [HttpPost]
        public IActionResult Create(TripCreateViewModel model)
        {
            if (!this.ModelState.IsValid)
                return this.Create();
            
            this.tripsService.Create(model, this.User.Identity.Name);
            return base.RedirectToAction("MyTrips", "Trips");
        }
        
        public IActionResult MyTrips()
        {
            var trips = this.tripsService.GetAllForUser(this.User.Identity.Name)
                .To<TripMyViewModel>()
                .ToList();

            return base.View(trips);
        }
        
        public IActionResult Details(int tripId)
        {
            Trip trip = this.tripsService.GetById(tripId);
            var model = this.mapper.Map<TripDetailsViewModel>(trip);
            return base.View(model);
        }
    }
}
