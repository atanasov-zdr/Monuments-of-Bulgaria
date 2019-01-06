namespace MB.Services.Contracts.Hotels
{
    using ViewModels.Hotels.HotelReviews;

    public interface IHotelReviewsService
    {
        void Create(HotelReviewWriteViewModel model, string username);

        bool CheckForExistingReview(int hotelId, string username);
    }
}
