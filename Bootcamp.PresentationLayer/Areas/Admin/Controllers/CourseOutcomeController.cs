using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CourseOutcomeController : Controller
    {
        private readonly ICourseOutcomeService _courseOutcomeService;
        private readonly ICourseService _courseService;
        private readonly IValidator<CourseOutcome> _validator;

        public CourseOutcomeController(ICourseOutcomeService courseOutcomeService, ICourseService courseService, IValidator<CourseOutcome> validator)
        {
            _courseOutcomeService = courseOutcomeService;
            _courseService = courseService;
            _validator = validator;
        }

        public IActionResult Index()
        {
            var outcomes = _courseOutcomeService.GetListBL();
            return View(outcomes);
        }

        public IActionResult Create()
        {
            ViewBag.Courses = _courseService.GetListBL();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CourseOutcome courseOutcome)
        {
            var validationResult = _validator.Validate(courseOutcome);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                ViewBag.Courses = _courseService.GetListBL();
                return View(courseOutcome);
            }

            _courseOutcomeService.InsertBL(courseOutcome);
            TempData["Success"] = "Kazanım başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var outcome = _courseOutcomeService.GetByIdBL(id);
            ViewBag.Courses = _courseService.GetListBL();
            return View(outcome);
        }

        [HttpPost]
        public IActionResult Edit(CourseOutcome courseOutcome)
        {
            var validationResult = _validator.Validate(courseOutcome);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                ViewBag.Courses = _courseService.GetListBL();
                return View(courseOutcome);
            }

            _courseOutcomeService.UpdateBL(courseOutcome);
            TempData["Success"] = "Kazanım başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var outcome = _courseOutcomeService.GetByIdBL(id);
            return View(outcome);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var outcome = _courseOutcomeService.GetByIdBL(id);
            _courseOutcomeService.DeleteBL(outcome);
            TempData["Success"] = "Kazanım başarıyla silindi.";
            return RedirectToAction("Index");
        }
    }
}