using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.BusinessLayer.Concrete
{
    public class CourseEnrollmentManager : ICourseEnrollmentService
    {
        private readonly ICourseEnrollmentDal _courseEnrollmentDal;

        public CourseEnrollmentManager(ICourseEnrollmentDal courseEnrollmentDal)
        {
            _courseEnrollmentDal = courseEnrollmentDal;
        }

        public void DeleteBL(CourseEnrollment t)
        {
            _courseEnrollmentDal.Delete(t);
        }

        public CourseEnrollment GetByIdBL(int id)
        {
            return _courseEnrollmentDal.GetById(id);
        }

        public List<CourseEnrollment> GetListBL()
        {
            return _courseEnrollmentDal.GetList();
        }

        public void InsertBL(CourseEnrollment t)
        {
            _courseEnrollmentDal.Insert(t);
        }

        public void UpdateBL(CourseEnrollment t)
        {
            _courseEnrollmentDal.Update(t);
        }

        public int GetEnrollmentCountByCourseId(int courseId)
        {
            return _courseEnrollmentDal.GetEnrollmentCountByCourseId(courseId);
        }

        public int GetEnrollmentCountByInstructorId(int instructorId)
        {
            return _courseEnrollmentDal.GetEnrollmentCountByInstructorId(instructorId);
        }
    }
} 