namespace MB.Services.Contracts.Hotels
{
    using System.Linq;

    using Models.Hotels;
    using ViewModels.Hotels;

    public interface IHotelsService
    {
        IQueryable<Hotel> GetAllOrderedByName();

        IQueryable<Hotel> GetAllForOblastOrderedByName(int oblastId);

        Hotel GetById(int hotelId);

        string GetNameById(int hotelId);

        int Add(HotelAddViewModel model);

        void Delete(int hotelId);

        void Update(HotelEditViewModel model);

        bool CheckExistById(int hotelId);
    }
}
