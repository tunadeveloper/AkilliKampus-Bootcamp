using System.ComponentModel.DataAnnotations;

namespace Bootcamp.EntityLayer.Concrete
{
    public class CourseEnrollment
    {
        [Key]
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        public int CourseId { get; set; }
        public Course Course { get; set; }
        
        public DateTime EnrolledAt { get; set; } = DateTime.Now;
    }
} 