using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using System.Collections.Generic;

namespace Bootcamp.BusinessLayer.Concrete
{
    public class CourseLevelManager : ICourseLevelService
    {
        private readonly ICourseLevelDal _courseLevelDal;

        public CourseLevelManager(ICourseLevelDal courseLevelDal)
        {
            _courseLevelDal = courseLevelDal;
        }

        public void DeleteBL(CourseLevel t)
        {
            _courseLevelDal.Delete(t);
        }

        public CourseLevel GetByIdBL(int id)
        {
            return _courseLevelDal.GetById(id);
        }

        public List<CourseLevel> GetListBL()
        {
            return _courseLevelDal.GetList();
        }

        public void InsertBL(CourseLevel t)
        {
            _courseLevelDal.Insert(t);
        }

        public void UpdateBL(CourseLevel t)
        {
            _courseLevelDal.Update(t);
        }
    }
}