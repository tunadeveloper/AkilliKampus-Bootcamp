using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.DataAccessLayer.Concrete;
using Bootcamp.DataAccessLayer.Repository;
using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.DataAccessLayer.EntityFramework
{
    public class EfCourseEnrollmentDal : GenericRepository<CourseEnrollment>, ICourseEnrollmentDal
    {
        public EfCourseEnrollmentDal(Context context) : base(context)
        {
        }
    }
} 