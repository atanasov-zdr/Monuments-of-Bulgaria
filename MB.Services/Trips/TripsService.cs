namespace MB.Services.Trips
{
    using System.Linq;

    using AutoMapper;

    using Common.Exceptions;
    using Common.Utilities;
    using Contracts.Hotels;
    using Contracts.Monuments;
    using Contracts.Trips;
    using Data;
    using Models;
    using Models.Monuments;
    using Models.Hotels;
    using Models.Trips;
    using Services.Contracts.Users;
    using ViewModels.Trips;

    public class TripsService : ITripsService
    {
        private const string ImagesDirectory = "wwwroot/images/trips/";
        private const string ImagesFolderName = "trips";

        private readonly MbDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ImagesUploader imagesUploader;
        private readonly IUsersService usersService;
        private readonly IMonumentsService monumentsService;
        private readonly IHotelsService hotelsService;

        public TripsService(
            MbDbContext dbContext,
            IMapper mapper,
            ImagesUploader imagesUploader,
            IUsersService usersService,
            IMonumentsService monumentsService,
            IHotelsService hotelsService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.imagesUploader = imagesUploader;
            this.usersService = usersService;
            this.monumentsService = monumentsService;
            this.hotelsService = hotelsService;
        }

        public void Create(TripCreateViewModel model, string username)
        {
            if (!this.monumentsService.CheckExistById(model.SelectedMonumentId))
                throw new MonumentNullException();

            if (!this.hotelsService.CheckExistById(model.SelectedHotelId))
                throw new HotelNullException();

            Trip trip = this.mapper.Map<Trip>(model);
            trip.ImageUrl = this.imagesUploader.Upload(model.Photo, ImagesDirectory, ImagesFolderName);

            MbUser user = this.usersService.GetByUsername(username);
            trip.User = user;

            this.dbContext.Trips.Add(trip);
            this.dbContext.SaveChanges();
        }

        public IQueryable<Trip> GetAllForUser(string username)
        {
            return this.dbContext.Trips
                .Where(x => x.User.UserName == username)
                .Where(x => x.IsDeleted == false);
        }

        public Trip GetById(int tripId)
        {
            Trip trip = this.dbContext.Trips.FirstOrDefault(x => x.Id == tripId);

            if (trip == null)
                throw new TripNullException();

            if (trip.IsDeleted == true)
                throw new TripDeletedException();

            return trip;
        }

        public bool CheckForTripOwn(int tripId, string username)
        {
            Trip trip = this.GetById(tripId);
            bool result = trip.User.UserName == username;
            return result;
        }

        public void Delete(int tripId)
        {
            Trip trip = this.GetById(tripId);
            trip.IsDeleted = true;
            this.dbContext.SaveChanges();
        }

        public void Update(TripEditViewModel model)
        {
            Monument monument = this.monumentsService.GetById(model.SelectedMonumentId);
            Hotel hotel = this.hotelsService.GetById(model.SelectedHotelId);

            Trip trip = this.GetById(model.Id);
            trip.Name = model.Name;
            trip.Description = model.Description;
            trip.Monument = monument;
            trip.Hotel = hotel;

            this.dbContext.SaveChanges();
        }
    }
}
