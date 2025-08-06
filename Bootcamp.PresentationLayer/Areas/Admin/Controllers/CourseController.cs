using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICourseCategoryService _categoryService;
        private readonly ICourseLevelService _levelService;
        private readonly IInstructorService _instructorService;
        private readonly IValidator<Course> _validator;

        public CourseController(ICourseService courseService, ICourseCategoryService categoryService, ICourseLevelService levelService, IInstructorService instructorService, IValidator<Course> validator)
        {
            _courseService = courseService;
            _categoryService = categoryService;
            _levelService = levelService;
            _instructorService = instructorService;
            _validator = validator;
        }

        public IActionResult Index()
        {
            var courses = _courseService.GetListBL();
            return View(courses);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _categoryService.GetListBL();
            ViewBag.Levels = _levelService.GetListBL();
            ViewBag.Instructors = _instructorService.GetListBL();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Course course)
        {
            var validationResult = _validator.Validate(course);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                ViewBag.Categories = _categoryService.GetListBL();
                ViewBag.Levels = _levelService.GetListBL();
                ViewBag.Instructors = _instructorService.GetListBL();
                return View(course);
            }

            _courseService.InsertBL(course);
            TempData["Success"] = "Kurs başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var course = _courseService.GetByIdBL(id);
            ViewBag.Categories = _categoryService.GetListBL();
            ViewBag.Levels = _levelService.GetListBL();
            ViewBag.Instructors = _instructorService.GetListBL();
            return View(course);
        }

        [HttpPost]
        public IActionResult Edit(Course course)
        {
            var validationResult = _validator.Validate(course);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                ViewBag.Categories = _categoryService.GetListBL();
                ViewBag.Levels = _levelService.GetListBL();
                ViewBag.Instructors = _instructorService.GetListBL();
                return View(course);
            }

            _courseService.UpdateBL(course);
            TempData["Success"] = "Kurs başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var course = _courseService.GetByIdBL(id);
            // Eğer ilişkili veriler null geliyorsa, gerekirse burada tekrar çekilebilir.
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _courseService.GetByIdBL(id);
            _courseService.DeleteBL(course);
            TempData["Success"] = "Kurs başarıyla silindi.";
            return RedirectToAction("Index");
        }
    }
}