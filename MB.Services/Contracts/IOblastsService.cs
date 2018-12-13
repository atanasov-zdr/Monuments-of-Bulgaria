namespace MB.Services.Contracts
{
    using System.Linq;

    using Models;

    public interface IOblastsService
    {
        IQueryable<Oblast> GetAllOrderedByName();
    }
}
