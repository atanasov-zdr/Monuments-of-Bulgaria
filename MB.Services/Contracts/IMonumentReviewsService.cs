namespace MB.Services.Contracts
{
    using MB.ViewModels.MonumentReviews;

    public interface IMonumentReviewsService
    {
        string GetNameById(int monumentId);

        void Create(MonumentReviewWriteViewModel model, string username);
    }
}
