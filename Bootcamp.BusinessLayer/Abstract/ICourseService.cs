using Bootcamp.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.BusinessLayer.Abstract
{
    public interface ICourseService : IGenericService<Course>
    {
        List<Course> GetCoursesWithCategoryBL();
        Course GetCourseWithAllBL(int id);
        List<Course> GetAllCoursesWithVideosAndProgressBL();

    }
}
