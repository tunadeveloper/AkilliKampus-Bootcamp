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
    public class CourseCategoryManager : ICourseCategoryService
    {
        private readonly ICourseCategoryDal _courseCategoryDal;

        public CourseCategoryManager(ICourseCategoryDal courseCategoryDal)
        {
            _courseCategoryDal = courseCategoryDal;
        }

        public void DeleteBL(CourseCategory t)
        {
           _courseCategoryDal.Delete(t);
        }

        public CourseCategory GetByIdBL(int id)
        {
          return _courseCategoryDal.GetById(id);
        }

        public List<CourseCategory> GetListBL()
        {
         return _courseCategoryDal.GetList();
        }

        public void InsertBL(CourseCategory t)
        {
           _courseCategoryDal.Insert(t);
        }

        public void UpdateBL(CourseCategory t)
        {
           _courseCategoryDal.Update(t);
        }
    }
}
