using Bootcamp.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.ViewComponents.Home
{
    public class _PopularCourseComponentPartial: ViewComponent
    {
        private readonly ICourseService _courseService;

        public _PopularCourseComponentPartial(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IViewComponentResult Invoke()
        {
            var values = _courseService.GetCoursesWithCategoryBL()
                .Where(x=>x.IsPopuler == true)
                .ToList();
            return View(values);
        }

    }
}
