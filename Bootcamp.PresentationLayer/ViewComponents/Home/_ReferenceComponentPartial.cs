using Bootcamp.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.ViewComponents.Home
{
    public class _ReferenceComponentPartial : ViewComponent
    {
        private readonly IReferenceService _referenceService;

        public _ReferenceComponentPartial(IReferenceService referenceService)
        {
            _referenceService = referenceService;
        }

        public IViewComponentResult Invoke()
        {
            var references = _referenceService.GetActiveReferences();
            return View(references);
        }
    }
}
