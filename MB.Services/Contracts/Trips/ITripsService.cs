namespace MB.Services.Contracts.Trips
{
    using System.Linq;

    using Models.Trips;
    using ViewModels.Trips;

    public interface ITripsService
    {
        void Create(TripCreateViewModel model, string username);

        IQueryable<Trip> GetAllForUser(string username);

        Trip GetById(int tripId);
    }
}
