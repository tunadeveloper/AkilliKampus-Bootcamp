using System.ComponentModel.DataAnnotations;

namespace Bootcamp.EntityLayer.Concrete
{
    public class VideoCompletion
    {
        [Key]
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        public int CourseId { get; set; }
        public Course Course { get; set; }
        
        public int CourseVideoId { get; set; }
        public CourseVideo CourseVideo { get; set; }
        
        public bool IsCompleted { get; set; } = false;
        
        public DateTime? CompletedAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
} 