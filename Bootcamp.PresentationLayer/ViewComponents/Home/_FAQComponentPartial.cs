using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.ViewComponents.Home
{
    public class _FAQComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
}
}
