namespace MB.Services.Contracts.Monuments
{
    using System.Linq;

    using Models.Monuments;
    using ViewModels.Monuments;

    public interface IMonumentsService
    {
        IQueryable<Monument> GetAllOrderedByName();

        IQueryable<Monument> GetAllForOblastOrderedByName(int oblastId);

        Monument GetById(int monumentId);

        int Add(MonumentAddViewModel model);

        void Delete(int monumentId);

        void Update(MonumentEditViewModel model);
    }
}
