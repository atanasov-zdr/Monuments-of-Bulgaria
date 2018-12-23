namespace MB.Services.Contracts.Monuments
{
    using System.Linq;

    using Models.Monuments;

    public interface IMonumentsService
    {
        IQueryable<Monument> GetAllOrderedByName();

        IQueryable<Monument> GetAllForOblastOrderedByName(int oblastId);

        Monument GetById(int monumentId);
    }
}
