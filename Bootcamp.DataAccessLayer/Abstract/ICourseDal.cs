using Bootcamp.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.DataAccessLayer.Abstract
{
    public interface ICourseDal : IGenericDal<Course>
    {
        List<Course> GetCoursesWithCategory();
        Course GetCourseWithAll(int id);
        List<Course> GetAllCoursesWithVideosAndProgress();

    }
}
