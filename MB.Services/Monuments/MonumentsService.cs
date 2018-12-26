namespace MB.Services.Monuments
{
    using System;
    using System.Linq;

    using Contracts.Monuments;
    using Data;
    using Models.Monuments;

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
            Monument monument = this.dbContext.Monuments.FirstOrDefault(x => x.Id == monumentId);

            if (monument == null)
                throw new ArgumentNullException(nameof(monument));

            return monument;
        }
    }
}
