namespace MB.Services.Contracts.Oblasts
{
    using System.Linq;

    using Models;

    public interface IOblastsService
    {
        IQueryable<Oblast> GetAllOrderedByName();
    }
}
