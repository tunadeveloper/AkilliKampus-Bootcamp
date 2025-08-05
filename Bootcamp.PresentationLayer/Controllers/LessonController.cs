using Bootcamp.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.Controllers
{
    public class LessonController : Controller
    {
        private readonly ICourseService _courseService;

        public LessonController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            var values = _courseService.GetCoursesWithCategoryBL();
            return View(values);
        }
    }
}
