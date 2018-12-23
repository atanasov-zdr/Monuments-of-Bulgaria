namespace MB.Services.Contracts.Hotels
{
    using System.Linq;

    using Models.Hotels;

    public interface IHotelsService
    {
        IQueryable<Hotel> GetAllOrderedByName();

        IQueryable<Hotel> GetAllForOblastOrderedByName(int oblastId);

        Hotel GetById(int hotelId);
    }
}
