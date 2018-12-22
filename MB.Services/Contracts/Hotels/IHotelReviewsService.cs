namespace MB.Services.Contracts.Hotels
{
    using ViewModels.Hotels.HotelReviews;

    public interface IHotelReviewsService
    {
        string GetNameById(int hotelId);

        void Create(HotelReviewWriteViewModel model, string username);
    }
}
