using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FAQController : Controller
    {
        private readonly IFAQService _faqService;
        private readonly IValidator<FAQ> _validator;

        public FAQController(IFAQService faqService, IValidator<FAQ> validator)
        {
            _faqService = faqService;
            _validator = validator;
        }

        public IActionResult Index()
        {
            var faqs = _faqService.GetListBL();
            return View(faqs);
        }

        public IActionResult Details(int id)
        {
            var faq = _faqService.GetByIdBL(id);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FAQ faq)
        {
            var validationResult = _validator.Validate(faq);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(faq);
            }

            faq.CreatedAt = DateTime.Now;
            _faqService.InsertBL(faq);
            TempData["Success"] = "SSS başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var faq = _faqService.GetByIdBL(id);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, FAQ faq)
        {
            if (id != faq.Id)
            {
                return NotFound();
            }

            var validationResult = _validator.Validate(faq);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(faq);
            }

            _faqService.UpdateBL(faq);
            TempData["Success"] = "SSS başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var faq = _faqService.GetByIdBL(id);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var faq = _faqService.GetByIdBL(id);
            if (faq == null)
            {
                return NotFound();
            }

            _faqService.DeleteBL(faq);
            TempData["Success"] = "SSS başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
} 