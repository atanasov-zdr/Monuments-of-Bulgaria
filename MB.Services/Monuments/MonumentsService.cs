namespace MB.Services.Monuments
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Contracts.Monuments;
    using Data;
    using Models.Monuments;
    using ViewModels.Monuments;

    public class MonumentsService : IMonumentsService
    {
        private readonly MbDbContext dbContext;
        private readonly IMapper mapper;

        public MonumentsService(MbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public IQueryable<Monument> GetAllOrderedByName()
        {
            return this.dbContext.Monuments.Where(x => x.IsDeleted == false).OrderBy(x => x.Name);
        }

        public IQueryable<Monument> GetAllForOblastOrderedByName(int oblastId)
        {
            return this.dbContext.Monuments.Where(x => x.OblastId == oblastId).Where(x => x.IsDeleted == false).OrderBy(x => x.Name);
        }

        public Monument GetById(int monumentId)
        {
            Monument monument = this.dbContext.Monuments.FirstOrDefault(x => x.Id == monumentId);

            if (monument == null)
                throw new ArgumentNullException(nameof(monument));

            if (monument.IsDeleted == true)
                throw new ArgumentNullException(nameof(monument));

            return monument;
        }

        public int Add(MonumentAddViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Monument monument = this.mapper.Map<Monument>(model);

            this.dbContext.Monuments.Add(monument);
            this.dbContext.SaveChanges();

            return monument.Id;
        }

        public void Delete(int monumentId)
        {
            Monument monument = this.GetById(monumentId);
            monument.IsDeleted = true;
            this.dbContext.SaveChanges();
        }

        public void Update(MonumentEditViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Monument monument = this.GetById(model.Id);
            monument.Name = model.Name;
            monument.Description = model.Description;
            monument.OblastId = model.SelectedOblastId;

            this.dbContext.SaveChanges();
        }
    }
}
