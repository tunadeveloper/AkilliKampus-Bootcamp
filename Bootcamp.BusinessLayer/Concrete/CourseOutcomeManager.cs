using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using System.Collections.Generic;

namespace Bootcamp.BusinessLayer.Concrete
{
    public class CourseOutcomeManager : ICourseOutcomeService
    {
        private readonly ICourseOutcomeDal _courseOutcomeDal;

        public CourseOutcomeManager(ICourseOutcomeDal courseOutcomeDal)
        {
            _courseOutcomeDal = courseOutcomeDal;
        }

        public void DeleteBL(CourseOutcome t)
        {
            _courseOutcomeDal.Delete(t);
        }

        public CourseOutcome GetByIdBL(int id)
        {
            return _courseOutcomeDal.GetById(id);
        }

        public List<CourseOutcome> GetListBL()
        {
            return _courseOutcomeDal.GetList();
        }

        public void InsertBL(CourseOutcome t)
        {
            _courseOutcomeDal.Insert(t);
        }

        public void UpdateBL(CourseOutcome t)
        {
            _courseOutcomeDal.Update(t);
        }
    }
}