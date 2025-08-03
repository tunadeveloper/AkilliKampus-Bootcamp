using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.BusinessLayer.Concrete
{
    public class CourseManager : ICourseService
    {
        private readonly ICourseDal _courseDal;

        public CourseManager(ICourseDal courseDal)
        {
            _courseDal = courseDal;
        }

        public void DeleteBL(Course t)
        {
          _courseDal.Delete(t);
        }

        public List<Course> GetAllCoursesWithVideosAndProgressBL()
        {
            return _courseDal.GetAllCoursesWithVideosAndProgress();
        }

        public Course GetByIdBL(int id)
        {
           return _courseDal.GetById(id);
        }

        public List<Course> GetCoursesWithCategoryBL()
        {
            return _courseDal.GetCoursesWithCategory();
        }

        public Course GetCourseWithAllBL(int id)
        {
            return _courseDal.GetCourseWithAll(id);
        }


        public List<Course> GetListBL()
        {
           return _courseDal.GetList();
        }

        public void InsertBL(Course t)
        {
            _courseDal.Insert(t);
        }

        public void UpdateBL(Course t)
        {
         _courseDal.Update(t);
        }
    }
}
