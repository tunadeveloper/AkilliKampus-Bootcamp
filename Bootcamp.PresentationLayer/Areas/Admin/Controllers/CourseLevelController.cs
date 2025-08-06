using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CourseLevelController : Controller
    {
        private readonly ICourseLevelService _courseLevelService;
        private readonly IValidator<CourseLevel> _validator;

        public CourseLevelController(ICourseLevelService courseLevelService, IValidator<CourseLevel> validator)
        {
            _courseLevelService = courseLevelService;
            _validator = validator;
        }

        public IActionResult Index()
        {
            var levels = _courseLevelService.GetListBL();
            return View(levels);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CourseLevel courseLevel)
        {
            var validationResult = _validator.Validate(courseLevel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(courseLevel);
            }

            _courseLevelService.InsertBL(courseLevel);
            TempData["Success"] = "Seviye başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var level = _courseLevelService.GetByIdBL(id);
            return View(level);
        }

        [HttpPost]
        public IActionResult Edit(CourseLevel courseLevel)
        {
            var validationResult = _validator.Validate(courseLevel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(courseLevel);
            }

            _courseLevelService.UpdateBL(courseLevel);
            TempData["Success"] = "Seviye başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var level = _courseLevelService.GetByIdBL(id);
            return View(level);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var level = _courseLevelService.GetByIdBL(id);
            if (level.Courses != null && level.Courses.Count > 0)
            {
                TempData["Error"] = "Bu seviyeye bağlı kurslar olduğu için silinemez.";
                return RedirectToAction("Delete", new { id });
            }
            _courseLevelService.DeleteBL(level);
            TempData["Success"] = "Seviye başarıyla silindi.";
            return RedirectToAction("Index");
        }
    }
}