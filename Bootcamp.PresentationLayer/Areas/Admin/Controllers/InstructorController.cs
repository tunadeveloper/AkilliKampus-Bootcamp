using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class InstructorController : Controller
    {
        private readonly IInstructorService _instructorService;
        private readonly IValidator<Instructor> _validator;

        public InstructorController(IInstructorService instructorService, IValidator<Instructor> validator)
        {
            _instructorService = instructorService;
            _validator = validator;
        }

        public IActionResult Index()
        {
            var instructors = _instructorService.GetListBL();
            return View(instructors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Instructor instructor)
        {
            var validationResult = _validator.Validate(instructor);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(instructor);
            }

            _instructorService.InsertBL(instructor);
            TempData["Success"] = "Eğitmen başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var instructor = _instructorService.GetByIdBL(id);
            return View(instructor);
        }

        [HttpPost]
        public IActionResult Edit(Instructor instructor)
        {
            var validationResult = _validator.Validate(instructor);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(instructor);
            }

            _instructorService.UpdateBL(instructor);
            TempData["Success"] = "Eğitmen başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var instructor = _instructorService.GetByIdBL(id);
            return View(instructor);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var instructor = _instructorService.GetByIdBL(id);
            if (instructor.Courses != null && instructor.Courses.Count > 0)
            {
                TempData["Error"] = "Bu eğitmene bağlı kurslar olduğu için silinemez.";
                return RedirectToAction("Delete", new { id });
            }
            _instructorService.DeleteBL(instructor);
            TempData["Success"] = "Eğitmen başarıyla silindi.";
            return RedirectToAction("Index");
        }
    }
}