using Bootcamp.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult StartLesson(int id)
        {
            var course = _courseService.GetCourseWithAllBL(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course); // Model olarak course gönderiyoruz
        }
    }
}
