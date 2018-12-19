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
        public IQueryable<Monument> GetAllForOblastOrderedByName(int oblastId)
        {
            return this.dbContext.Monuments.Where(x => x.OblastId == oblastId).OrderBy(x => x.Name);
        }

        public Monument GetById(int monumentId)
        {
            return this.dbContext.Monuments.FirstOrDefault(x => x.Id == monumentId);
        }
    }
}
