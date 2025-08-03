using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.EntityLayer.Concrete
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string ThumbnailUrl { get; set; }

        public int CategoryId { get; set; }
        public CourseCategory Category { get; set; }

        public int CourseLevelId { get; set; }
        public CourseLevel CourseLevel { get; set; }

        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }

        public ICollection<CourseOutcome> Outcomes { get; set; }
        public ICollection<CourseVideo> CourseVideos { get; set; }
        public ICollection<Progress> Progresses { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }


    }
}
