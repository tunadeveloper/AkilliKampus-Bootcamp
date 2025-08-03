using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using Bootcamp.PresentationLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace Bootcamp.PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICommentService _commentService;
        private readonly IProgressService _progressService;
        private readonly ICourseEnrollmentService _courseEnrollmentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICourseVideoService _courseVideoService; // Added for AddSampleVideoDurations

        public HomeController(ICourseService courseService, ICommentService commentService, IProgressService progressService, ICourseEnrollmentService courseEnrollmentService, UserManager<ApplicationUser> userManager, ICourseVideoService courseVideoService)
        {
            _courseService = courseService;
            _commentService = commentService;
            _progressService = progressService;
            _courseEnrollmentService = courseEnrollmentService;
            _userManager = userManager;
            _courseVideoService = courseVideoService; // Initialize for AddSampleVideoDurations
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CourseDetail(int id)
        {
            var course = _courseService.GetCourseWithAllBL(id);
            var filteredComments = course.Comments?.Where(x => x.CourseId == id).ToList();
            ViewBag.Comments = filteredComments;

            var user = await _userManager.GetUserAsync(User);

            // Kullanıcının bu eğitime kayıtlı olup olmadığını kontrol et
            bool isRegistered = false;
            int lastWatchedVideoId = 0;
            if (user != null)
            {
                // Önce enrollment kontrolü
                var enrollment = _courseEnrollmentService.GetListBL()
                    .FirstOrDefault(e => e.UserId == user.Id && e.CourseId == id);
                
                isRegistered = enrollment != null;
                
                // Eğer kayıtlıysa, son izlenen videoyu bul
                if (isRegistered && course.CourseVideos != null)
                {
                    var userProgresses = _progressService.GetListBL()
                        .Where(p => p.UserId == user.Id && p.CourseId == id)
                        .OrderByDescending(p => p.CompletedAt)
                        .ToList();
                    
                    if (userProgresses.Any())
                    {
                        lastWatchedVideoId = userProgresses.First().CourseVideoId;
                    }
                }
            }

            ViewBag.IsRegistered = isRegistered;
            ViewBag.LastWatchedVideoId = lastWatchedVideoId;

            return View(course);
        }

        // Eğitime kayıt olma action'ı
        [HttpPost]
        public async Task<IActionResult> EnrollToCourse(int courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Giriş yapmanız gerekiyor" });
            }

            try
            {
                var course = _courseService.GetCourseWithAllBL(courseId);
                if (course == null)
                {
                    return Json(new { success = false, message = "Eğitim bulunamadı" });
                }

                // Kullanıcının zaten kayıtlı olup olmadığını kontrol et
                var existingEnrollment = _courseEnrollmentService.GetListBL()
                    .FirstOrDefault(e => e.UserId == user.Id && e.CourseId == courseId);

                if (existingEnrollment != null)
                {
                    return Json(new { success = false, message = "Bu eğitime zaten kayıtlısınız" });
                }

                // Eğitime kayıt ol
                var newEnrollment = new CourseEnrollment
                {
                    UserId = user.Id,
                    CourseId = courseId,
                    EnrolledAt = DateTime.Now
                };

                _courseEnrollmentService.InsertBL(newEnrollment);
                return Json(new { success = true, message = "Eğitime başarıyla kayıt oldunuz", redirectUrl = $"/Course/StartLesson/{courseId}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }

        // Örnek video süreleri eklemek için (sadece geliştirme amaçlı)
        [HttpPost]
        public IActionResult AddSampleVideoDurations()
        {
            try
            {
                var courseVideos = _courseVideoService.GetListBL();
                
                // Örnek süreler (saniye cinsinden)
                var durations = new int[] { 1800, 2400, 2100, 2700, 1950, 2250, 2550, 1800, 2400, 2100 }; // 30dk, 40dk, 35dk, 45dk, 32.5dk, 37.5dk, 42.5dk, 30dk, 40dk, 35dk
                
                for (int i = 0; i < courseVideos.Count && i < durations.Length; i++)
                {
                    courseVideos[i].Duration = durations[i];
                    _courseVideoService.UpdateBL(courseVideos[i]);
                }
                
                return Json(new { success = true, message = "Video süreleri eklendi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata: " + ex.Message });
            }
        }

    }
}

