using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CourseCategoryController : Controller
    {
        private readonly ICourseCategoryService _courseCategoryService;
        private readonly IValidator<CourseCategory> _validator;

        public CourseCategoryController(ICourseCategoryService courseCategoryService, IValidator<CourseCategory> validator)
        {
            _courseCategoryService = courseCategoryService;
            _validator = validator;
        }

        public IActionResult Index()
        {
            var categories = _courseCategoryService.GetListBL();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CourseCategory courseCategory)
        {
            var validationResult = _validator.Validate(courseCategory);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(courseCategory);
            }

            _courseCategoryService.InsertBL(courseCategory);
            TempData["Success"] = "Kategori başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var category = _courseCategoryService.GetByIdBL(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(CourseCategory courseCategory)
        {
            var validationResult = _validator.Validate(courseCategory);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(courseCategory);
            }

            _courseCategoryService.UpdateBL(courseCategory);
            TempData["Success"] = "Kategori başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var category = _courseCategoryService.GetByIdBL(id);
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _courseCategoryService.GetByIdBL(id);
            if (category.Courses != null && category.Courses.Count > 0)
            {
                TempData["Error"] = "Bu kategoriye bağlı kurslar olduğu için silinemez.";
                return RedirectToAction("Delete", new { id });
            }
            _courseCategoryService.DeleteBL(category);
            TempData["Success"] = "Kategori başarıyla silindi.";
            return RedirectToAction("Index");
        }
    }
} 