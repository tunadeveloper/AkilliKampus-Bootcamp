using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.DataAccessLayer.Concrete;
using Bootcamp.DataAccessLayer.Repository;
using Bootcamp.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp.DataAccessLayer.EntityFramework
{
    public class EfCourseEnrollmentDal : GenericRepository<CourseEnrollment>, ICourseEnrollmentDal
    {
        public EfCourseEnrollmentDal(Context context) : base(context)
        {
        }

        public int GetEnrollmentCountByCourseId(int courseId)
        {
            using var context = new Context();
            return context.CourseEnrollments
                .Where(e => e.CourseId == courseId)
                .Count();
        }

        public int GetEnrollmentCountByInstructorId(int instructorId)
        {
            using var context = new Context();
            return context.CourseEnrollments
                .Include(e => e.Course)
                .Where(e => e.Course.InstructorId == instructorId)
                .Count();
        }
    }
} 