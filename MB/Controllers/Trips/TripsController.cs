namespace MB.Controllers.Trips
{
    using System.IO;
    using System.Linq;

    using AutoMapper;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using Common;
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
                return base.View(model);
            
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
            if (!this.tripsService.CheckForTripOwn(tripId, this.User.Identity.Name))
            {
                string errorMsg = $"Trip with id {tripId} is not your trip!";
                return base.View(GlobalConstants.ErrorViewName, errorMsg);
            }

            Trip trip = this.tripsService.GetById(tripId);
            var model = this.mapper.Map<TripDetailsViewModel>(trip);
            return base.View(model);
        }

        public IActionResult Edit(int tripId)
        {
            if (!this.tripsService.CheckForTripOwn(tripId, this.User.Identity.Name))
            {
                string errorMsg = $"Trip with id {tripId} is not your trip!";
                return base.View(GlobalConstants.ErrorViewName, errorMsg);
            }

            Trip trip = this.tripsService.GetById(tripId);
            var viewModel = this.mapper.Map<TripEditViewModel>(trip);

            var hotels = this.hotelsService.GetAllOrderedByName()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            viewModel.Hotels = hotels;
            var monuments = this.monumentsService.GetAllOrderedByName()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
            viewModel.Monuments = monuments;

            return base.View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(TripEditViewModel model)
        {
            if (!this.ModelState.IsValid)
                return base.RedirectToAction("Edit", new { tripId = model.Id });
            
            if (!this.tripsService.CheckForTripOwn(model.Id, this.User.Identity.Name))
            {
                string errorMsg = $"Trip with id {model.Id} is not your trip!";
                return base.View(GlobalConstants.ErrorViewName, errorMsg);
            }

            this.tripsService.Update(model);

            return base.RedirectToAction("Details", new { tripId = model.Id });
        }

        [HttpPost]
        public IActionResult Delete(int tripId)
        {
            if (!this.tripsService.CheckForTripOwn(tripId, this.User.Identity.Name))
            {
                string errorMsg = $"Trip with id {tripId} is not your trip!";
                return base.View(GlobalConstants.ErrorViewName, errorMsg);
            }

            this.tripsService.Delete(tripId);

            return base.RedirectToAction("MyTrips");
        }
    }
}
