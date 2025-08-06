using Bootcamp.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.ViewComponents.Shared
{
    public class _SiteSettingsComponentPartial : ViewComponent
    {
        private readonly ISiteSettingService _siteSettingService;

        public _SiteSettingsComponentPartial(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        public IViewComponentResult Invoke()
        {
            var seoSettings = _siteSettingService.GetByGroup("SEO");
            var logoSettings = _siteSettingService.GetByGroup("Logo");
            var generalSettings = _siteSettingService.GetByGroup("General");
            var contactSettings = _siteSettingService.GetByGroup("Contact");
            var socialSettings = _siteSettingService.GetByGroup("Social");

            ViewBag.SEOSettings = seoSettings;
            ViewBag.LogoSettings = logoSettings;
            ViewBag.GeneralSettings = generalSettings;
            ViewBag.ContactSettings = contactSettings;
            ViewBag.SocialSettings = socialSettings;

            return View();
        }
    }
} 