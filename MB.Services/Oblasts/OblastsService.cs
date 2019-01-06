namespace MB.Services.Oblasts
{
    using System.Linq;
    
    using Contracts.Oblasts;
    using Data;
    using Models.Oblasts;

    public class OblastsService : IOblastsService
    {
        private readonly MbDbContext dbContext;

        public OblastsService(MbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Oblast> GetAllOrderedByName()
        {
            return this.dbContext.Oblasts.OrderBy(x => x.Name);
        }

        public bool CheckExistById(int oblastId)
        {
            bool result = this.dbContext.Oblasts.Any(x => x.Id == oblastId);
            return result;
        }
    }
}
