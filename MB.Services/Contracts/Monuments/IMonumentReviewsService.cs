namespace MB.Services.Contracts.Monuments
{
    using MB.ViewModels.Monuments.MonumentReviews;

    public interface IMonumentReviewsService
    {
        void Create(MonumentReviewWriteViewModel model, string username);

        bool CheckForExistingReview(int monumentId, string username);
    }
}
