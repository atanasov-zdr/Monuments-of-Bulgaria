namespace MB.Services.Trips
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Common.Utilities;
    using Contracts.Trips;
    using Data;
    using Models;
    using Models.Trips;
    using ViewModels.Trips;

    public class TripsService : ITripsService
    {
        private const string ImagesDirectory = "wwwroot/images/trips/";
        private const string ImagesFolderName = "trips";

        private readonly MbDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ImagesUploader imagesUploader;

        public TripsService(MbDbContext dbContext, IMapper mapper, ImagesUploader imagesUploader)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.imagesUploader = imagesUploader;
        }

        public void Create(TripCreateViewModel model, string username)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (username == null)
                throw new ArgumentNullException(nameof(username));

            MbUser user = this.dbContext.Users.FirstOrDefault(x => x.UserName == username);

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            Trip trip = this.mapper.Map<Trip>(model);
            trip.UserId = user.Id;
            trip.ImageUrl = this.imagesUploader.Upload(model.Photo, ImagesDirectory, ImagesFolderName);

            this.dbContext.Trips.Add(trip);
            this.dbContext.SaveChanges();
        }

        public IQueryable<Trip> GetAllForUser(string username)
        {
            return this.dbContext.Trips.Where(x => x.User.UserName == username).Where(x => x.IsDeleted == false);
        }

        public Trip GetById(int tripId)
        {
            Trip trip = this.dbContext.Trips.FirstOrDefault(x => x.Id == tripId);

            if (trip == null)
                throw new ArgumentNullException(nameof(trip));

            if (trip.IsDeleted == true)
                throw new ArgumentNullException(nameof(trip));

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
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Trip trip = this.GetById(model.Id);
            trip.Name = model.Name;
            trip.Description = model.Description;
            trip.MonumentId = model.SelectedMonumentId;
            trip.HotelId = model.SelectedHotelId;

            this.dbContext.SaveChanges();
        }
    }
}
