using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.ViewComponents.Home
{
    public class _ReferenceComponentPartial: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
