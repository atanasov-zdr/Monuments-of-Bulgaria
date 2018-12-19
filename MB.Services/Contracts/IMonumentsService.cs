namespace MB.Services.Contracts
{
    using System.Linq;

    using Models;

    public interface IMonumentsService
    {
        IQueryable<Monument> GetAllOrderedByName();

        IQueryable<Monument> GetAllForOblastOrderedByName(int oblastId);

        Monument GetById(int monumentId);
    }
}
