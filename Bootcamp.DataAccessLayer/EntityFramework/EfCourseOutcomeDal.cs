using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.DataAccessLayer.Concrete;
using Bootcamp.DataAccessLayer.Repository;
using Bootcamp.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.DataAccessLayer.EntityFramework
{
    public class EfCourseOutcomeDal: GenericRepository<CourseOutcome>, ICourseOutcomeDal
    {
        public EfCourseOutcomeDal(Context context) : base(context)
        { }
    }
}
