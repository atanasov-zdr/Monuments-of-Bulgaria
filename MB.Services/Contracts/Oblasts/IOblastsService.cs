namespace MB.Services.Contracts.Oblasts
{
    using System.Linq;

    using Models.Oblasts;

    public interface IOblastsService
    {
        IQueryable<Oblast> GetAllOrderedByName();

        bool CheckExistById(int oblastId);
    }
}
