namespace MB.Services.Contracts.Hotels
{
    using System.Linq;

    using Models.Hotels;

    public interface IHotelCommentsService
    {
        IQueryable<HotelComment> GetAllForHotelOrderedByDateDescending(int hotelId);

        void Create(int hotelId, string content, string username);

        void Like(int commentId, string username);
    }
}
