using Bootcamp.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.ViewComponents.Home
{
    public class _InstructorComponentPartial: ViewComponent
    {
        private readonly IInstructorService _instructorService;

        public _InstructorComponentPartial(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public IViewComponentResult Invoke()
        {
            var values = _instructorService.GetListBL();
            return View(values);
        }
    }
}
