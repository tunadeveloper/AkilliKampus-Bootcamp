using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.EntityLayer.Concrete
{
    public class Instructor
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string NameSurname { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string LinkedinUrl { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
