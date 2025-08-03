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
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ICourseService courseService, ICommentService commentService, UserManager<ApplicationUser> userManager)
        {
            _courseService = courseService;
            _commentService = commentService;
            _userManager = userManager;
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

            // Null korumasý yapýldý
            bool isRegistered = course.CourseVideos?
                .SelectMany(v => v.Progresses ?? new List<Progress>())
                .Any(p => p.UserId == user.Id) ?? false;

            ViewBag.IsRegistered = isRegistered;

            return View(course);

        }

    }
}

