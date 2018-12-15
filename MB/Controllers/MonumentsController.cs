namespace MB.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using ReflectionIT.Mvc.Paging;

    using Base;
    using Mapping;
    using Services.Contracts;
    using ViewModels.Monuments;

    public class MonumentsController : BaseController
    {
        private readonly IMonumentsService monumentsService;

        public MonumentsController(IMonumentsService monumentsService)
        {
            this.monumentsService = monumentsService;
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
            return base.View();
        }
    }
}
