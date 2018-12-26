namespace MB.Services.Trips
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Contracts.Trips;
    using Data;
    using Models;
    using Models.Trips;
    using ViewModels.Trips;

    public class TripsService : ITripsService
    {
        private readonly MbDbContext dbContext;
        private readonly IMapper mapper;

        public TripsService(MbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
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

            this.dbContext.Trips.Add(trip);
            this.dbContext.SaveChanges();
        }

        public IQueryable<Trip> GetAllForUser(string username)
        {
            return this.dbContext.Trips.Where(x => x.User.UserName == username);
        }

        public Trip GetById(int tripId)
        {
            Trip trip = this.dbContext.Trips.FirstOrDefault(x => x.Id == tripId);

            if (trip == null)
                throw new ArgumentNullException(nameof(trip));

            return trip;
        }
    }
}
