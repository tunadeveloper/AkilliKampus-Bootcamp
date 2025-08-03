using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.EntityLayer.Concrete
{
    public class CourseVideo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string Summary { get; set; }
        public int Duration { get; set; } // Video süresi (saniye cinsinden)

        public int CourseID { get; set; }
        public Course Course { get; set; }

        public ICollection<Progress> Progresses { get; set; }
    }
}
