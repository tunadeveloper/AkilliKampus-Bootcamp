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
        private readonly ICourseVideoService _courseVideoService;
        private readonly IVideoCompletionService _videoCompletionService;

        public HomeController(ICourseService courseService, ICommentService commentService, IProgressService progressService, ICourseEnrollmentService courseEnrollmentService, UserManager<ApplicationUser> userManager, ICourseVideoService courseVideoService, IVideoCompletionService videoCompletionService)
        {
            _courseService = courseService;
            _commentService = commentService;
            _progressService = progressService;
            _courseEnrollmentService = courseEnrollmentService;
            _userManager = userManager;
            _courseVideoService = courseVideoService; 
            _videoCompletionService = videoCompletionService;
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

            bool isRegistered = false;
            int lastWatchedVideoId = 0;
            if (user != null)
            {
                var enrollment = _courseEnrollmentService.GetListBL()
                    .FirstOrDefault(e => e.UserId == user.Id && e.CourseId == id);
                
                isRegistered = enrollment != null;
                
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

            int courseEnrollmentCount = _courseEnrollmentService.GetEnrollmentCountByCourseId(id);
            ViewBag.CourseEnrollmentCount = courseEnrollmentCount;

            int instructorEnrollmentCount = _courseEnrollmentService.GetEnrollmentCountByInstructorId(course.InstructorId);
            ViewBag.InstructorEnrollmentCount = instructorEnrollmentCount;

            int videoCount = course.CourseVideos?.Count ?? 0;
            ViewBag.VideoCount = videoCount;

            int completedVideos = 0;
            int totalVideos = videoCount;
            double progressPercentage = 0;

            if (user != null && isRegistered && totalVideos > 0)
            {
                var userVideoCompletions = _videoCompletionService.GetUserVideoCompletions(user.Id, id);
                completedVideos = userVideoCompletions.Count(vc => vc.IsCompleted);
                progressPercentage = (double)completedVideos / totalVideos * 100;
            }

            ViewBag.CompletedVideos = completedVideos;
            ViewBag.TotalVideos = totalVideos;
            ViewBag.ProgressPercentage = Math.Round(progressPercentage, 1);

            return View(course);
        }

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

                var existingEnrollment = _courseEnrollmentService.GetListBL()
                    .FirstOrDefault(e => e.UserId == user.Id && e.CourseId == courseId);

                if (existingEnrollment != null)
                {
                    return Json(new { success = false, message = "Bu eğitime zaten kayıtlısınız" });
                }

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

        [HttpPost]
        public IActionResult AddSampleVideoDurations()
        {
            try
            {
                var courseVideos = _courseVideoService.GetListBL();
                
                var durations = new int[] { 1800, 2400, 2100, 2700, 1950, 2250, 2550, 1800, 2400, 2100 }; 
                
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

        [HttpPost]
        public async Task<IActionResult> SendComment(int courseId, string commentText)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Giriş yapmanız gerekiyor" });
            }

            if (string.IsNullOrWhiteSpace(commentText))
            {
                return Json(new { success = false, message = "Yorum metni boş olamaz" });
            }

            try
            {
                var course = _courseService.GetCourseWithAllBL(courseId);
                if (course == null)
                {
                    return Json(new { success = false, message = "Eğitim bulunamadı" });
                }

                var newComment = new Comment
                {
                    Text = commentText,
                    CourseId = courseId,
                    ApplicationUserId = user.Id,
                    CreatedAt = DateTime.Now
                };

                _commentService.InsertBL(newComment);

                return Json(new { 
                    success = true, 
                    message = "Yorumunuz başarıyla gönderildi",
                    comment = new {
                        text = newComment.Text,
                        userName = user.NameSurname,
                        userGender = user.Gender,
                        createdAt = newComment.CreatedAt.ToString("dd MMMM yyyy")
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }

    }
}

