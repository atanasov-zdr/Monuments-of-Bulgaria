namespace MB.Services.Oblasts
{
    using System.Linq;
    
    using Contracts.Oblasts;
    using Data;
    using Models;

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
    }
}
