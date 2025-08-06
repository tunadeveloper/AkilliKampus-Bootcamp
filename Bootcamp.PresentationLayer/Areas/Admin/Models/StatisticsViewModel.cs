using System;
using System.Collections.Generic;

namespace Bootcamp.PresentationLayer.Areas.Admin.Models
{
    public class StatisticsViewModel
    {
        // Genel İstatistikler
        public int TotalUsers { get; set; }
        public int TotalCourses { get; set; }
        public int TotalComments { get; set; }
        public int TotalCategories { get; set; }
        public int TotalLevels { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalOutcomes { get; set; }

        // Popüler Kurslar
        public List<PopularCourseViewModel> PopularCourses { get; set; }

        // Kategori Dağılımı
        public List<CategoryDistributionViewModel> CategoryDistribution { get; set; }

        // Seviye Dağılımı
        public List<LevelDistributionViewModel> LevelDistribution { get; set; }

        // Eğitmen Performansı
        public List<InstructorPerformanceViewModel> InstructorPerformance { get; set; }

        // Son Yorumlar
        public List<RecentCommentViewModel> RecentComments { get; set; }

        // Aylık İstatistikler
        public List<MonthlyStatViewModel> MonthlyStats { get; set; }
    }

    public class PopularCourseViewModel
    {
        public string CourseName { get; set; }
        public int CommentCount { get; set; }
        public string CategoryName { get; set; }
    }

    public class CategoryDistributionViewModel
    {
        public string CategoryName { get; set; }
        public int CourseCount { get; set; }
    }

    public class LevelDistributionViewModel
    {
        public string LevelName { get; set; }
        public int CourseCount { get; set; }
    }

    public class InstructorPerformanceViewModel
    {
        public string InstructorName { get; set; }
        public int CourseCount { get; set; }
        public string Position { get; set; }
    }

    public class RecentCommentViewModel
    {
        public string UserName { get; set; }
        public string CourseName { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class MonthlyStatViewModel
    {
        public string Month { get; set; }
        public int NewUsers { get; set; }
        public int NewComments { get; set; }
        public int NewCourses { get; set; }
    }
} 