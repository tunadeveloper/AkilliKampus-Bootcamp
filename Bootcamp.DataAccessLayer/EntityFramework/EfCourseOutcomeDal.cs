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
    public class EfCourseOutcomeDal: GenericRepository<CourseOutcome>, ICourseOutcomeDal
    {
        public EfCourseOutcomeDal(Context context) : base(context)
        { }
        private Context Context => (Context)_context;
        public override List<CourseOutcome> GetList()
        {
            return Context.CourseOutcomes.Include(o => o.Course).ToList();
        }
        public override CourseOutcome GetById(int id)
        {
            return Context.CourseOutcomes.Include(o => o.Course).FirstOrDefault(o => o.Id == id);
        }
    }
}
