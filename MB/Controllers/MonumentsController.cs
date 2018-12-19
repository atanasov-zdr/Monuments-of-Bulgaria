namespace MB.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using ReflectionIT.Mvc.Paging;

    using Base;
    using Mapping;
    using Models;
    using Services.Contracts;
    using ViewModels.Monuments;

    public class MonumentsController : BaseController
    {
        private readonly IMonumentsService monumentsService;
        private readonly IMapper mapper;

        public MonumentsController(IMonumentsService monumentsService, IMapper mapper)
        {
            this.monumentsService = monumentsService;
            this.mapper = mapper;
        }

        public IActionResult All(int page = 1)
        {
            IEnumerable<MonumentAllViewModel> monuments = this.monumentsService
                .GetAllOrderedByName()
                .To<MonumentAllViewModel>()
                .ToList();

            int pageSize = 12;
            IPagingList<MonumentAllViewModel> viewModel = PagingList.Create(monuments, pageSize, page);
            return View(viewModel);            
        }

        public IActionResult AllForOblast(int oblastId, int page = 1)
        {
            IEnumerable<MonumentAllViewModel> monuments = this.monumentsService
                .GetAllForOblastOrderedByName(oblastId)
                .To<MonumentAllViewModel>()
                .ToList();

            int pageSize = 12;
            IPagingList<MonumentAllViewModel> viewModel = PagingList.Create(monuments, pageSize, page);
            return base.View(viewModel);
        }        

        public IActionResult Details(int monumentId)
        {
            Monument monument = this.monumentsService.GetById(monumentId);

            if (monument == null)
                throw new ArgumentNullException();

            var viewModel = this.mapper.Map<MonumentDetailsViewModel>(monument);

            return base.View(viewModel);
        }
    }
}
