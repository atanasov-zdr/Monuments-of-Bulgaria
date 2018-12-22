namespace MB.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using Base;
    using Mapping;
    using Services.Contracts.Oblasts;
    using ViewModels.Oblasts;

    public class OblastsController : BaseController
    {
        private readonly IOblastsService oblastsService;

        public OblastsController(IOblastsService oblastsService)
        {
            this.oblastsService = oblastsService;
        }

        public IActionResult All()
        {
            IEnumerable<OblastAllViewModel> viewModel = this.oblastsService
                .GetAllOrderedByName()
                .To<OblastAllViewModel>()
                .ToList();
            return base.View(viewModel);
        }
    }
}
