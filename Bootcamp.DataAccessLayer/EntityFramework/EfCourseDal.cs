using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.DataAccessLayer.Concrete;
using Bootcamp.DataAccessLayer.Repository;
using Bootcamp.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.DataAccessLayer.EntityFramework
{
    public class EfCourseDal: GenericRepository<Course>, ICourseDal
    {
        private readonly Context _context;
        public EfCourseDal(Context context) : base(context)
        {
            _context = context;
        }

        public List<Course> GetCoursesWithCategory()
        {
            return _context.Courses
                .Include(x=>x.Category)
                .ToList();
        }

        public Course GetCourseWithAll(int id)
        {
            return _context.Courses
      .Include(x => x.Category)
      .Include(x => x.Comments)
          .ThenInclude(c => c.ApplicationUser)
      .Include(x => x.CourseLevel)
      .Include(x => x.Instructor)
      .Include(x => x.Outcomes)
      .Include(x => x.CourseVideos)
      .FirstOrDefault(x => x.Id == id);
        }
        public List<Course> GetAllCoursesWithVideosAndProgress()
        {
            using var context = new Context();
            return context.Courses
                .Include(c => c.CourseVideos)
                    .ThenInclude(cv => cv.Progresses)
                .ToList();
        }


    }
}
