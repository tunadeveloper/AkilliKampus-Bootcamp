using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.EntityLayer.Concrete
{
    public class CourseOutcome
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
