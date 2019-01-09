namespace MB.Services.Monuments
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Common.Exceptions;
    using Common.Utilities;
    using Contracts.Monuments;
    using Contracts.Oblasts;
    using Data;
    using Models.Monuments;
    using ViewModels.Monuments;

    public class MonumentsService : IMonumentsService
    {
        private const string ImagesDirectory = "wwwroot/images/monuments/";
        private const string ImagesFolderName = "monuments";

        private readonly MbDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ImagesUploader imagesUploader;
        private readonly IOblastsService oblastsService;

        public MonumentsService(MbDbContext dbContext,
            IMapper mapper,
            ImagesUploader imagesUploader,
            IOblastsService oblastsService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.imagesUploader = imagesUploader;
            this.oblastsService = oblastsService;
        }

        public IQueryable<Monument> GetAllOrderedByName()
        {
            return this.dbContext.Monuments.Where(x => x.IsDeleted == false).OrderBy(x => x.Name);
        }

        public IQueryable<Monument> GetAllForOblastOrderedByName(int oblastId)
        {
            return this.dbContext.Monuments
                .Where(x => x.OblastId == oblastId)
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Name);
        }

        public Monument GetById(int monumentId)
        {
            Monument monument = this.dbContext.Monuments.FirstOrDefault(x => x.Id == monumentId);

            if (monument == null)
                throw new MonumentNullException();

            if (monument.IsDeleted == true)
                throw new MonumentDeletedException();

            return monument;
        }

        public string GetNameById(int monumentId)
        {
            Monument monument = this.GetById(monumentId);
            
            if (monument.Name == null)
                throw new ArgumentNullException(nameof(monument.Name));

            return monument.Name;
        }

        public int Add(MonumentAddViewModel model)
        {
            if (!this.oblastsService.CheckExistById(model.SelectedOblastId))
                throw new OblastNullException();

            Monument monument = this.mapper.Map<Monument>(model);
            monument.ImageUrl = this.imagesUploader.Upload(model.Photo, ImagesDirectory, ImagesFolderName);

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
            if (!this.oblastsService.CheckExistById(model.SelectedOblastId))
                throw new OblastNullException();

            Monument monument = this.GetById(model.Id);
            monument.Name = model.Name;
            monument.Description = model.Description;
            monument.OblastId = model.SelectedOblastId;

            if (model.Photo != null)
                monument.ImageUrl = this.imagesUploader.Upload(model.Photo, ImagesDirectory, ImagesFolderName);

            this.dbContext.SaveChanges();
        }

        public bool CheckExistById(int monumentId)
        {
            bool result = this.dbContext.Monuments.Any(x => x.Id == monumentId);
            return result;
        }
    }
}
