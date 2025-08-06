using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReferenceController : Controller
    {
        private readonly IReferenceService _referenceService;
        private readonly IValidator<Reference> _validator;

        public ReferenceController(IReferenceService referenceService, IValidator<Reference> validator)
        {
            _referenceService = referenceService;
            _validator = validator;
        }

        public IActionResult Index()
        {
            var references = _referenceService.GetListBL();
            return View(references);
        }

        public IActionResult Details(int id)
        {
            var reference = _referenceService.GetByIdBL(id);
            if (reference == null)
            {
                return NotFound();
            }
            return View(reference);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Reference reference)
        {
            var validationResult = _validator.Validate(reference);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(reference);
            }

            reference.CreatedAt = DateTime.Now;
            _referenceService.InsertBL(reference);
            TempData["Success"] = "Referans başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var reference = _referenceService.GetByIdBL(id);
            if (reference == null)
            {
                return NotFound();
            }
            return View(reference);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Reference reference)
        {
            if (id != reference.Id)
            {
                return NotFound();
            }

            var validationResult = _validator.Validate(reference);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(reference);
            }

            _referenceService.UpdateBL(reference);
            TempData["Success"] = "Referans başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var reference = _referenceService.GetByIdBL(id);
            if (reference == null)
            {
                return NotFound();
            }
            return View(reference);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var reference = _referenceService.GetByIdBL(id);
            if (reference == null)
            {
                return NotFound();
            }

            _referenceService.DeleteBL(reference);
            TempData["Success"] = "Referans başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
} 