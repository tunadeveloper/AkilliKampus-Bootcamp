using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.DataAccessLayer.Concrete;
using Bootcamp.DataAccessLayer.Repository;
using Bootcamp.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp.DataAccessLayer.EntityFramework
{
    public class EfCourseLevelDal : GenericRepository<CourseLevel>, ICourseLevelDal
    {
        public EfCourseLevelDal(Context context) : base(context)
        { }

        private Context Context => (Context)_context;
        public CourseLevel GetById(int id)
        {
            return Context.CourseLevels.Include(cl => cl.Courses).FirstOrDefault(cl => cl.Id == id);
        }

        public override List<CourseLevel> GetList()
        {
            return Context.CourseLevels.Include(cl => cl.Courses).ToList();
        }
    }
}
