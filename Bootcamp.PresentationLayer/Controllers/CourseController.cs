using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.PresentationLayer.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICourseVideoService _courseVideoService;
        private readonly IProgressService _progressService;
        private readonly IVideoCompletionService _videoCompletionService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(ICourseService courseService, ICourseVideoService courseVideoService, IProgressService progressService, IVideoCompletionService videoCompletionService, UserManager<ApplicationUser> userManager)
        {
            _courseService = courseService;
            _courseVideoService = courseVideoService;
            _progressService = progressService;
            _videoCompletionService = videoCompletionService;
            _userManager = userManager;
        }

        public async Task<IActionResult> StartLesson(int id)
        {
            var course = _courseService.GetCourseWithAllBL(id);
            if (course == null)
            {
                return NotFound();
            }

            // Kullanıcının giriş yapıp yapmadığını kontrol et
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // ViewBag'e video bilgilerini ekle
            if (course.CourseVideos != null && course.CourseVideos.Any())
            {
                // Kullanıcının bu eğitimdeki video tamamlanma durumlarını bul
                var userVideoCompletions = _videoCompletionService.GetUserVideoCompletions(user.Id, id);

                CourseVideo currentVideo;
                if (userVideoCompletions.Any())
                {
                    // Son tamamlanan videoyu bul
                    var lastCompletedVideo = userVideoCompletions
                        .Where(vc => vc.IsCompleted)
                        .OrderByDescending(vc => vc.CompletedAt)
                        .FirstOrDefault();
                    
                    if (lastCompletedVideo != null)
                    {
                        var lastCompletedVideoEntity = course.CourseVideos.FirstOrDefault(v => v.Id == lastCompletedVideo.CourseVideoId);
                        
                        if (lastCompletedVideoEntity != null)
                        {
                            var lastCompletedIndex = course.CourseVideos.ToList().IndexOf(lastCompletedVideoEntity);
                            
                            // Eğer son video değilse, bir sonraki videoyu al
                            if (lastCompletedIndex < course.CourseVideos.Count - 1)
                            {
                                currentVideo = course.CourseVideos.ElementAt(lastCompletedIndex + 1);
                            }
                            else
                            {
                                // Son videoyu tekrar göster
                                currentVideo = lastCompletedVideoEntity;
                            }
                        }
                        else
                        {
                            // Video bulunamadıysa ilk videoyu göster
                            currentVideo = course.CourseVideos.First();
                        }
                    }
                    else
                    {
                        // Hiç tamamlanmış video yoksa ilk videoyu göster
                        currentVideo = course.CourseVideos.First();
                    }
                }
                else
                {
                    // Hiç video tamamlanma kaydı yoksa ilk videoyu göster
                    currentVideo = course.CourseVideos.First();
                }

                // YouTube URL'sini embed formatına çevir
                currentVideo.VideoUrl = ConvertToEmbedUrl(currentVideo.VideoUrl);
                ViewBag.CurrentVideo = currentVideo;
                
                var videoList = course.CourseVideos.ToList();
                foreach (var video in videoList)
                {
                    video.VideoUrl = ConvertToEmbedUrl(video.VideoUrl);
                }
                ViewBag.VideoList = videoList;
                
                // Kullanıcının tamamladığı video ID'lerini ViewBag'e ekle
                var completedVideoIds = userVideoCompletions
                    .Where(vc => vc.IsCompleted)
                    .Select(vc => vc.CourseVideoId)
                    .ToList();
                ViewBag.CompletedVideoIds = completedVideoIds;
                

            }

            return View(course);
        }

        // Video tamamlanma durumunu kaydet
        [HttpPost]
        public async Task<IActionResult> SaveProgress(int courseId, int videoId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı giriş yapmamış" });
            }

            try
            {
                // Video tamamlanma durumunu kaydet
                _videoCompletionService.MarkVideoAsCompleted(user.Id, courseId, videoId);

                return Json(new { success = true, message = "Video tamamlandı olarak işaretlendi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }

        // Video tamamlanma durumunu kaldır
        [HttpPost]
        public async Task<IActionResult> RemoveProgress(int courseId, int videoId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı giriş yapmamış" });
            }

            try
            {
                // Video tamamlanma durumunu kaldır
                _videoCompletionService.MarkVideoAsIncomplete(user.Id, courseId, videoId);

                return Json(new { success = true, message = "Video tamamlanma durumu kaldırıldı" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }

        // YouTube URL'sini embed formatına çeviren yardımcı metod
        private string ConvertToEmbedUrl(string videoUrl)
        {
            if (string.IsNullOrEmpty(videoUrl))
                return videoUrl;

            if (videoUrl.Contains("youtube.com/watch?v="))
            {
                var videoId = videoUrl.Split("v=")[1];
                if (videoId.Contains("&"))
                    videoId = videoId.Split("&")[0];
                return $"https://www.youtube.com/embed/{videoId}";
            }
            else if (videoUrl.Contains("youtu.be/"))
            {
                var videoId = videoUrl.Split("youtu.be/")[1];
                if (videoId.Contains("?"))
                    videoId = videoId.Split("?")[0];
                return $"https://www.youtube.com/embed/{videoId}";
            }
            else if (videoUrl.Contains("youtube.com/embed/"))
            {
                return videoUrl; // Zaten embed formatında
            }

            return videoUrl; // Desteklenmeyen format
        }
    }
}
