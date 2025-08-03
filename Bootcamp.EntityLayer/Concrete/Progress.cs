using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.EntityLayer.Concrete
{
    public class Progress
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int CourseVideoId { get; set; }
        public CourseVideo CourseVideo { get; set; }

        public DateTime CompletedAt { get; set; }
    }
}
