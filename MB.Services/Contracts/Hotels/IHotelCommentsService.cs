namespace MB.Services.Contracts.Hotels
{
    using System.Linq;

    using Models;

    public interface IHotelCommentsService
    {
        IQueryable<HotelComment> GetAllForHotel(int hotelId);

        void Create(int hotelId, string content, string username);

        void Like(int commentId, string username);
    }
}
