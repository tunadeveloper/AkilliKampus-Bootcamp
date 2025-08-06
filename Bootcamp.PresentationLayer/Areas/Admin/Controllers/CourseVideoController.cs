using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bootcamp.PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CourseVideoController : Controller
    {
        private readonly ICourseVideoService _courseVideoService;
        private readonly ICourseService _courseService;

        public CourseVideoController(ICourseVideoService courseVideoService, ICourseService courseService)
        {
            _courseVideoService = courseVideoService;
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            var courseVideos = _courseVideoService.GetListBL();
            return View(courseVideos);
        }

        public IActionResult Create()
        {
            ViewBag.Courses = new SelectList(_courseService.GetListBL(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(CourseVideo courseVideo)
        {
            if (ModelState.IsValid)
            {
                _courseVideoService.InsertBL(courseVideo);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Courses = new SelectList(_courseService.GetListBL(), "Id", "Name");
            return View(courseVideo);
        }

        public IActionResult Edit(int id)
        {
            var courseVideo = _courseVideoService.GetByIdBL(id);
            if (courseVideo == null)
            {
                return NotFound();
            }
            ViewBag.Courses = new SelectList(_courseService.GetListBL(), "Id", "Name");
            return View(courseVideo);
        }

        [HttpPost]
        public IActionResult Edit(int id, CourseVideo courseVideo)
        {
            if (id != courseVideo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _courseVideoService.UpdateBL(courseVideo);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Courses = new SelectList(_courseService.GetListBL(), "Id", "Name");
            return View(courseVideo);
        }

        public IActionResult Details(int id)
        {
            var courseVideo = _courseVideoService.GetByIdBL(id);
            if (courseVideo == null)
            {
                return NotFound();
            }
            return View(courseVideo);
        }

        public IActionResult Delete(int id)
        {
            var courseVideo = _courseVideoService.GetByIdBL(id);
            if (courseVideo == null)
            {
                return NotFound();
            }
            return View(courseVideo);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var courseVideo = _courseVideoService.GetByIdBL(id);
            if (courseVideo != null)
            {
                _courseVideoService.DeleteBL(courseVideo);
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 