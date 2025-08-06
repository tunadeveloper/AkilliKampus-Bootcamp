using Bootcamp.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.ViewComponents.Home
{
    public class _FAQComponentPartial : ViewComponent
    {
        private readonly IFAQService _faqService;

        public _FAQComponentPartial(IFAQService faqService)
        {
            _faqService = faqService;
        }

        public IViewComponentResult Invoke()
        {
            var faqs = _faqService.GetActiveFAQs();
            return View(faqs);
        }
    }
}
