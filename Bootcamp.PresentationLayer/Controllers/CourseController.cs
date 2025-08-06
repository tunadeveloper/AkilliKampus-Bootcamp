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
        private readonly IGeminiService _geminiService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(ICourseService courseService, ICourseVideoService courseVideoService, IProgressService progressService, IVideoCompletionService videoCompletionService, IGeminiService geminiService, UserManager<ApplicationUser> userManager)
        {
            _courseService = courseService;
            _courseVideoService = courseVideoService;
            _progressService = progressService;
            _videoCompletionService = videoCompletionService;
            _geminiService = geminiService;
            _userManager = userManager;
        }

        public async Task<IActionResult> StartLesson(int id)
        {
            var course = _courseService.GetCourseWithAllBL(id);
            if (course == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (course.CourseVideos != null && course.CourseVideos.Any())
            {
                var userVideoCompletions = _videoCompletionService.GetUserVideoCompletions(user.Id, id);

                CourseVideo currentVideo;
                if (userVideoCompletions.Any())
                {
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
                            
                            if (lastCompletedIndex < course.CourseVideos.Count - 1)
                            {
                                currentVideo = course.CourseVideos.ElementAt(lastCompletedIndex + 1);
                            }
                            else
                            {
                                currentVideo = lastCompletedVideoEntity;
                            }
                        }
                        else
                        {
                            currentVideo = course.CourseVideos.First();
                        }
                    }
                    else
                    {
                        currentVideo = course.CourseVideos.First();
                    }
                }
                else
                {
                    currentVideo = course.CourseVideos.First();
                }

                currentVideo.VideoUrl = ConvertToEmbedUrl(currentVideo.VideoUrl);
                ViewBag.CurrentVideo = currentVideo;
                
                var videoList = course.CourseVideos.ToList();
                foreach (var video in videoList)
                {
                    video.VideoUrl = ConvertToEmbedUrl(video.VideoUrl);
                }
                ViewBag.VideoList = videoList;
                
                var completedVideoIds = userVideoCompletions
                    .Where(vc => vc.IsCompleted)
                    .Select(vc => vc.CourseVideoId)
                    .ToList();
                ViewBag.CompletedVideoIds = completedVideoIds;

                int totalVideos = course.CourseVideos.Count;
                int completedVideos = completedVideoIds.Count;
                double progressPercentage = totalVideos > 0 ? (double)completedVideos / totalVideos * 100 : 0;

                ViewBag.TotalVideos = totalVideos;
                ViewBag.CompletedVideos = completedVideos;
                ViewBag.ProgressPercentage = Math.Round(progressPercentage, 1);

            }

            return View(course);
        }

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
                _videoCompletionService.MarkVideoAsCompleted(user.Id, courseId, videoId);

                return Json(new { success = true, message = "Video tamamlandı olarak işaretlendi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }

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
                _videoCompletionService.MarkVideoAsIncomplete(user.Id, courseId, videoId);

                return Json(new { success = true, message = "Video tamamlanma durumu kaldırıldı" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SummarizeVideo([FromBody] SummarizeVideoRequest request)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "Kullanıcı giriş yapmamış" });
                }

                if (request == null)
                {
                    return Json(new { success = false, message = "Geçersiz istek" });
                }

                var video = _courseVideoService.GetByIdBL(request.VideoId);
                if (video == null)
                {
                    return Json(new { success = false, message = "Video bulunamadı" });
                }

                var summary = await _geminiService.SummarizeVideoAsync(video.VideoUrl, video.Name, video.Description);

                return Json(new { success = true, summary = summary });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Video özetlenirken hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DownloadPdf([FromBody] DownloadPdfRequest request)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "Kullanıcı giriş yapmamış" });
                }

                if (request == null || string.IsNullOrEmpty(request.Summary))
                {
                    return Json(new { success = false, message = "Geçersiz istek veya özet bulunamadı" });
                }

                var video = _courseVideoService.GetByIdBL(request.VideoId);
                if (video == null)
                {
                    return Json(new { success = false, message = "Video bulunamadı" });
                }

                var course = _courseService.GetByIdBL(request.CourseId);
                if (course == null)
                {
                    return Json(new { success = false, message = "Kurs bulunamadı" });
                }

                var pdfBytes = await _geminiService.GeneratePdfFromSummaryAsync(
                    request.Summary, 
                    course.Name, 
                    video.Name
                );

                string fileName = $"{course.Name}_{video.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                fileName = fileName.Replace(" ", "_").Replace("/", "_").Replace("\\", "_");

                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "PDF oluşturulurken hata oluştu: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetApiUsage()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "Kullanıcı giriş yapmamış" });
                }

                var (used, total) = await _geminiService.GetApiUsageAsync();
                var remaining = total - used;

                return Json(new { 
                    success = true, 
                    used = used, 
                    total = total, 
                    remaining = remaining,
                    percentage = Math.Round((double)used / total * 100, 1)
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckApiLimit()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "Kullanıcı giriş yapmamış" });
                }

                var testRequest = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = "Merhaba"
                                }
                            }
                        }
                    }
                };

                var json = System.Text.Json.JsonSerializer.Serialize(testRequest);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                using var httpClient = new HttpClient();
                var apiKey = "YOUR_API_KEY"; 
                var apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";

                var response = await httpClient.PostAsync($"{apiUrl}?key={apiKey}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "API kullanılabilir" });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    return Json(new { success = false, message = "API limit aşıldı", limitExceeded = true });
                }
                else
                {
                    return Json(new { success = false, message = "API hatası", limitExceeded = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }

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
                return videoUrl; 
            }

            return videoUrl;
        }

        [HttpPost]
        public async Task<IActionResult> AskQuestion([FromBody] AskQuestionRequest request)
        {
            try
            {
                Console.WriteLine($"AskQuestion called with request: {request?.VideoId}, Question: {request?.Question}");
                
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    Console.WriteLine("User not found");
                    return Json(new { success = false, message = "Kullanıcı giriş yapmamış" });
                }

                if (request == null)
                {
                    Console.WriteLine("Request is null");
                    return Json(new { success = false, message = "Geçersiz istek" });
                }

                var video = _courseVideoService.GetByIdBL(request.VideoId);
                if (video == null)
                {
                    Console.WriteLine($"Video not found for ID: {request.VideoId}");
                    return Json(new { success = false, message = "Video bulunamadı" });
                }

                if (string.IsNullOrWhiteSpace(request.Question))
                {
                    Console.WriteLine("Question is empty");
                    return Json(new { success = false, message = "Soru boş olamaz" });
                }

                Console.WriteLine($"Calling Gemini API with question: {request.Question}");
                var answer = await _geminiService.AskQuestionAsync(request.Question, video.Name, video.Description);
                Console.WriteLine($"Gemini API response: {answer}");

                return Json(new { success = true, answer = answer });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in AskQuestion: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return Json(new { success = false, message = "Soru cevaplanırken hata oluştu: " + ex.Message });
            }
        }

        public class AskQuestionRequest
        {
            public int VideoId { get; set; }
            public string Question { get; set; }
        }

        public class SummarizeVideoRequest
        {
            public int VideoId { get; set; }
        }

        public class DownloadPdfRequest
        {
            public int VideoId { get; set; }
            public int CourseId { get; set; }
            public string Summary { get; set; }
        }
    }
}
