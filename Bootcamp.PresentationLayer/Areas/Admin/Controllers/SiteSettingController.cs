using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SiteSettingController : Controller
    {
        private readonly ISiteSettingService _siteSettingService;
        private readonly IValidator<SiteSetting> _validator;

        public SiteSettingController(ISiteSettingService siteSettingService, IValidator<SiteSetting> validator)
        {
            _siteSettingService = siteSettingService;
            _validator = validator;
        }

        public IActionResult Settings()
        {
            // Varsayılan ayarları oluştur
            CreateDefaultSettings();
            
            ViewBag.SEOSettings = _siteSettingService.GetByGroup("SEO");
            ViewBag.LogoSettings = _siteSettingService.GetByGroup("Logo");
            ViewBag.GeneralSettings = _siteSettingService.GetByGroup("General");
            ViewBag.ContactSettings = _siteSettingService.GetByGroup("Contact");
            ViewBag.SocialSettings = _siteSettingService.GetByGroup("Social");

            return View();
        }

        [HttpPost]
        public IActionResult UpdateSetting(string key, string value)
        {
            try
            {
                var setting = _siteSettingService.GetByKey(key);
                if (setting != null)
                {
                    setting.Value = value;
                    setting.UpdatedAt = DateTime.Now;
                    _siteSettingService.UpdateBL(setting);
                    TempData["Success"] = "Ayar başarıyla güncellendi.";
                }
                else
                {
                    TempData["Error"] = "Ayar bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Güncelleme sırasında hata oluştu: " + ex.Message;
            }

            return RedirectToAction("Settings");
        }

        private void CreateDefaultSettings()
        {
            var defaultSettings = new List<SiteSetting>
            {
                // SEO Ayarları
                new SiteSetting { Key = "SiteTitle", Value = "Yapay Zeka Destekli Eğitim Platformu", Description = "Site Başlığı", Group = "SEO" },
                new SiteSetting { Key = "SiteDescription", Value = "Geleceğin eğitimini bugünden yaşa!", Description = "Site Açıklaması", Group = "SEO" },
                new SiteSetting { Key = "SiteKeywords", Value = "yapay zeka, eğitim, online kurs, sertifika", Description = "Site Anahtar Kelimeleri", Group = "SEO" },
                new SiteSetting { Key = "SiteAuthor", Value = "Akıllı Kampus", Description = "Site Sahibi", Group = "SEO" },
                
                // Logo Ayarları
                new SiteSetting { Key = "LogoText", Value = "Yapay Zeka Eğitim", Description = "Logo Metni", Group = "Logo" },
                new SiteSetting { Key = "LogoIcon", Value = "bi-lightbulb", Description = "Logo İkonu", Group = "Logo" },
                new SiteSetting { Key = "LogoColor", Value = "#6F42C1", Description = "Logo Rengi", Group = "Logo" },
                
                // Genel Ayarlar
                new SiteSetting { Key = "SiteName", Value = "Akıllı Kampus", Description = "Site Adı", Group = "General" },
                new SiteSetting { Key = "SiteSlogan", Value = "Geleceğin eğitimini bugünden yaşa!", Description = "Site Sloganı", Group = "General" },
                new SiteSetting { Key = "CopyrightText", Value = "© 2025 Akıllı Kampus. Tüm hakları saklıdır.", Description = "Telif Hakkı Metni", Group = "General" },
                
                // İletişim Ayarları
                new SiteSetting { Key = "ContactEmail", Value = "info@akillikampus.com", Description = "İletişim E-posta", Group = "Contact" },
                new SiteSetting { Key = "ContactPhone", Value = "+90 555 123 45 67", Description = "İletişim Telefon", Group = "Contact" },
                new SiteSetting { Key = "ContactAddress", Value = "İstanbul, Türkiye", Description = "İletişim Adres", Group = "Contact" },
                
                // Sosyal Medya Ayarları
                new SiteSetting { Key = "FacebookUrl", Value = "", Description = "Facebook URL", Group = "Social" },
                new SiteSetting { Key = "TwitterUrl", Value = "", Description = "Twitter URL", Group = "Social" },
                new SiteSetting { Key = "InstagramUrl", Value = "", Description = "Instagram URL", Group = "Social" },
                new SiteSetting { Key = "LinkedInUrl", Value = "", Description = "LinkedIn URL", Group = "Social" },
                new SiteSetting { Key = "YouTubeUrl", Value = "", Description = "YouTube URL", Group = "Social" }
            };

            foreach (var setting in defaultSettings)
            {
                var existingSetting = _siteSettingService.GetByKey(setting.Key);
                if (existingSetting == null)
                {
                    _siteSettingService.InsertBL(setting);
                }
            }
        }

        // CRUD Actions
        public IActionResult Index()
        {
            var settings = _siteSettingService.GetListBL();
            return View(settings);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SiteSetting siteSetting)
        {
            var validationResult = _validator.Validate(siteSetting);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(siteSetting);
            }

            _siteSettingService.InsertBL(siteSetting);
            TempData["Success"] = "Site ayarı başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var setting = _siteSettingService.GetByIdBL(id);
            if (setting == null)
            {
                return NotFound();
            }
            return View(setting);
        }

        [HttpPost]
        public IActionResult Edit(SiteSetting siteSetting)
        {
            var validationResult = _validator.Validate(siteSetting);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(siteSetting);
            }

            siteSetting.UpdatedAt = DateTime.Now;
            _siteSettingService.UpdateBL(siteSetting);
            TempData["Success"] = "Site ayarı başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var setting = _siteSettingService.GetByIdBL(id);
            if (setting == null)
            {
                return NotFound();
            }
            return View(setting);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var setting = _siteSettingService.GetByIdBL(id);
            if (setting != null)
            {
                _siteSettingService.DeleteBL(setting);
                TempData["Success"] = "Site ayarı başarıyla silindi.";
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var setting = _siteSettingService.GetByIdBL(id);
            if (setting == null)
            {
                return NotFound();
            }
            return View(setting);
        }
    }
} 