using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.EntityLayer.Concrete
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string NameSurname { get; set; }
        public string Gender { get; set; }
        public string? GeminiApiKey { get; set; }

        public ICollection<Progress> Progresses { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Course> Courses { get; set; }

    }
}
