namespace MB.Services
{
    using System.Linq;

    using Contracts;
    using Data;
    using Models;

    public class MonumentsService : IMonumentsService
    {
        private readonly MbDbContext dbContext;

        public MonumentsService(MbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Monument> GetAllOrderedByName()
        {
            return this.dbContext.Monuments.OrderBy(x => x.Name);
        }
    }
}
