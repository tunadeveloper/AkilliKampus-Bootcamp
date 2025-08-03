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

        public UserProfileController(UserManager<ApplicationUser> userManager, ICourseService courseService)
        {
            _userManager = userManager;
            _courseService = courseService;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var progresses = _courseService.GetAllCoursesWithVideosAndProgressBL()
     .Where(p => p.CourseVideos.Any(cv => cv.Progresses.Any(pr => pr.UserId == user.Id)))
     .ToList();

            var courseStatuses = progresses
                .GroupBy(p => p) // p zaten Course nesnesi
                .Select(g =>
                {
                    var course = g.Key;
                    int total = course.CourseVideos?.Count ?? 0;
                    int watched = course.CourseVideos?.Count(v =>
                        v.Progresses.Any(p => p.UserId == user.Id)) ?? 0;

                    string status = (watched == total && total > 0) ? "Tamamlandı" : "Devam Ediyor";

                    return (course, status, watched, total);
                }).ToList();


            ViewBag.CourseStatuses = courseStatuses;
            return View(user);
        }

    }
}
