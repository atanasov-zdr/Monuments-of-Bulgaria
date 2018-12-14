namespace MB.Services.Contracts
{
    using System.Linq;

    using Models;

    public interface IMonumentsService
    {
        IQueryable<Monument> GetAllOrderedByName();
    }
}
