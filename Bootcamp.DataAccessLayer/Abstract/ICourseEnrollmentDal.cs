using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.DataAccessLayer.Abstract
{
    public interface ICourseEnrollmentDal : IGenericDal<CourseEnrollment>
    {
        int GetEnrollmentCountByCourseId(int courseId);
        int GetEnrollmentCountByInstructorId(int instructorId);
    }
} 