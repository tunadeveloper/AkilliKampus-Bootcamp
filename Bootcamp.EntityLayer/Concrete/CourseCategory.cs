using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.EntityLayer.Concrete
{
    public class CourseCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
