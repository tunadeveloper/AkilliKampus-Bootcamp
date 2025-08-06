using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.BusinessLayer.Abstract
{
    public interface ICourseEnrollmentService : IGenericService<CourseEnrollment>
    {
        int GetEnrollmentCountByCourseId(int courseId);
        int GetEnrollmentCountByInstructorId(int instructorId);
    }
} 