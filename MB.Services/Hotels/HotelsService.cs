namespace MB.Services.Hotels
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Contracts.Hotels;
    using Data;
    using Models.Hotels;
    using ViewModels.Hotels;

    public class HotelsService : IHotelsService
    {
        private readonly MbDbContext dbContext;
        private readonly IMapper mapper;

        public HotelsService(MbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
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
            Hotel hotel = this.dbContext.Hotels.FirstOrDefault(x => x.Id == hotelId);

            if (hotel == null)
                throw new ArgumentNullException(nameof(hotel));

            return hotel;
        }

        public int Add(HotelAddViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Hotel hotel = this.mapper.Map<Hotel>(model);

            this.dbContext.Hotels.Add(hotel);
            this.dbContext.SaveChanges();

            return hotel.Id;
        }
    }
}
