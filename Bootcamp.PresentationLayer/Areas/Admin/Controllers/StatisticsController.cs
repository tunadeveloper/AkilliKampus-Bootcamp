using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using Bootcamp.PresentationLayer.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Bootcamp.PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StatisticsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ICourseService _courseService;
        private readonly ICourseCategoryService _categoryService;
        private readonly ICourseLevelService _levelService;
        private readonly IInstructorService _instructorService;
        private readonly ICourseOutcomeService _outcomeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Bootcamp.DataAccessLayer.Concrete.Context _context;

        public StatisticsController(
            ICommentService commentService,
            ICourseService courseService,
            ICourseCategoryService categoryService,
            ICourseLevelService levelService,
            IInstructorService instructorService,
            ICourseOutcomeService outcomeService,
            UserManager<ApplicationUser> userManager,
            Bootcamp.DataAccessLayer.Concrete.Context context)
        {
            _commentService = commentService;
            _courseService = courseService;
            _categoryService = categoryService;
            _levelService = levelService;
            _instructorService = instructorService;
            _outcomeService = outcomeService;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var statistics = new StatisticsViewModel
            {
                TotalUsers = _userManager.Users.Count(),
                TotalCourses = _courseService.GetListBL().Count,
                TotalComments = _commentService.GetListBL().Count,
                TotalCategories = _categoryService.GetListBL().Count,
                TotalLevels = _levelService.GetListBL().Count,
                TotalInstructors = _instructorService.GetListBL().Count,
                TotalOutcomes = _outcomeService.GetListBL().Count,

                PopularCourses = _context.Courses
                    .Include(c => c.Comments)
                    .Include(c => c.Category)
                    .OrderByDescending(c => c.Comments.Count)
                    .Take(5)
                    .Select(c => new PopularCourseViewModel
                    {
                        CourseName = c.Name,
                        CommentCount = c.Comments.Count,
                        CategoryName = c.Category != null ? c.Category.Name : "Kategori Yok"
                    })
                    .ToList(),

                CategoryDistribution = _context.CourseCategories
                    .Include(cc => cc.Courses)
                    .Select(cc => new CategoryDistributionViewModel
                    {
                        CategoryName = cc.Name,
                        CourseCount = cc.Courses.Count
                    })
                    .ToList(),

                LevelDistribution = _context.CourseLevels
                    .Include(cl => cl.Courses)
                    .Select(cl => new LevelDistributionViewModel
                    {
                        LevelName = cl.Name,
                        CourseCount = cl.Courses.Count
                    })
                    .ToList(),

                InstructorPerformance = _context.Instructors
                    .Include(i => i.Courses)
                    .Select(i => new InstructorPerformanceViewModel
                    {
                        InstructorName = i.NameSurname,
                        CourseCount = i.Courses.Count,
                        Position = i.Position
                    })
                    .OrderByDescending(ip => ip.CourseCount)
                    .Take(5)
                    .ToList(),

                RecentComments = _context.Comments
                    .Include(c => c.ApplicationUser)
                    .Include(c => c.Course)
                    .OrderByDescending(c => c.CreatedAt)
                    .Take(5)
                    .Select(c => new RecentCommentViewModel
                    {
                        UserName = c.ApplicationUser != null ? c.ApplicationUser.NameSurname : "Anonim",
                        CourseName = c.Course != null ? c.Course.Name : "Kurs Yok",
                        CommentText = c.Text.Length > 50 ? c.Text.Substring(0, 50) + "..." : c.Text,
                        CreatedAt = c.CreatedAt
                    })
                    .ToList(),

                MonthlyStats = GetPlatformStatistics()
            };

            return View(statistics);
        }

        private List<MonthlyStatViewModel> GetPlatformStatistics()
        {
            var platformStats = new List<MonthlyStatViewModel>();

            var popularCategories = _context.CourseCategories
                .Include(cc => cc.Courses)
                .OrderByDescending(cc => cc.Courses.Count)
                .Take(3)
                .ToList();

            foreach (var category in popularCategories)
            {
                platformStats.Add(new MonthlyStatViewModel
                {
                    Month = category.Name,
                    NewUsers = category.Courses.Count,
                    NewComments = _context.Comments.Count(c => c.Course.CategoryId == category.Id),
                    NewCourses = category.Courses.Count
                });
            }

            var popularLevels = _context.CourseLevels
                .Include(cl => cl.Courses)
                .OrderByDescending(cl => cl.Courses.Count)
                .Take(3)
                .ToList();

            foreach (var level in popularLevels)
            {
                platformStats.Add(new MonthlyStatViewModel
                {
                    Month = level.Name,
                    NewUsers = level.Courses.Count,
                    NewComments = _context.Comments.Count(c => c.Course.CourseLevelId == level.Id),
                    NewCourses = level.Courses.Count
                });
            }

            return platformStats;
        }
    }
} 