using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICourseService _courseService;
        private readonly IProgressService _progressService;
        private readonly ICourseEnrollmentService _courseEnrollmentService;
        private readonly IVideoCompletionService _videoCompletionService;

        public UserProfileController(UserManager<ApplicationUser> userManager, ICourseService courseService, IProgressService progressService, ICourseEnrollmentService courseEnrollmentService, IVideoCompletionService videoCompletionService)
        {
            _userManager = userManager;
            _courseService = courseService;
            _progressService = progressService;
            _courseEnrollmentService = courseEnrollmentService;
            _videoCompletionService = videoCompletionService;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            // Kullanıcının kayıtlı olduğu kursları al
            var userEnrollments = _courseEnrollmentService.GetListBL()
                .Where(e => e.UserId == user.Id)
                .ToList();

            var userVideoCompletions = _videoCompletionService.GetListBL()
                .Where(vc => vc.UserId == user.Id)
                .ToList();

            var courseStatuses = new List<dynamic>();

            foreach (var enrollment in userEnrollments)
            {
                var course = _courseService.GetCourseWithAllBL(enrollment.CourseId);
                if (course != null)
                {
                    int totalVideos = course.CourseVideos?.Count ?? 0;
                    
                    // Bu kurs için kullanıcının video tamamlanma durumlarını bul
                    var courseVideoCompletions = userVideoCompletions.Where(vc => vc.CourseId == course.Id && vc.IsCompleted).ToList();
                    int watchedVideos = courseVideoCompletions.Count;

                    // Yüzde hesapla
                    double percentage = totalVideos > 0 ? (double)watchedVideos / totalVideos * 100 : 0;
                    
                    // Durum belirle
                    string status;
                    if (totalVideos == 0)
                    {
                        status = "Henüz Başlanmadı";
                    }
                    else if (watchedVideos == 0)
                    {
                        status = "Henüz Başlanmadı";
                    }
                    else if (watchedVideos == totalVideos)
                    {
                        status = "Tamamlandı";
                    }
                    else
                    {
                        status = "Devam Ediyor";
                    }

                    courseStatuses.Add(new
                    {
                        Course = course,
                        Status = status,
                        WatchedCount = watchedVideos,
                        TotalCount = totalVideos,
                        Percentage = (double)Math.Round(percentage, 1)
                    });
                }
            }

            // Yüzdeye göre sırala
            courseStatuses = courseStatuses.OrderByDescending(x => x.Percentage).ToList();

            ViewBag.CourseStatuses = courseStatuses;
            return View(user);
        }

    }
}
