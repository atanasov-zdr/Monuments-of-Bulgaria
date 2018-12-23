namespace MB.Services.Hotels
{
    using System.Linq;

    using Contracts.Hotels;
    using Data;
    using Models.Hotels;

    public class HotelsService : IHotelsService
    {
        private readonly MbDbContext dbContext;

        public HotelsService(MbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Hotel> GetAllOrderedByName()
        {
            return this.dbContext.Hotels.OrderBy(x => x.Name);
        }

        public IQueryable<Hotel> GetAllForOblastOrderedByName(int oblastId)
        {
            return this.dbContext.Hotels.Where(x => x.OblastId == oblastId).OrderBy(x => x.Name);
        }

        public Hotel GetById(int hotelId)
        {
            return this.dbContext.Hotels.FirstOrDefault(x => x.Id == hotelId);
        }
    }
}
