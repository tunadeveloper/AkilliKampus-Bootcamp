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
    public class EfCourseCategoryDal : GenericRepository<CourseCategory>, ICourseCategoryDal
    {
        public EfCourseCategoryDal(Context context) : base(context)
        {
        }

        private Context Context => (Context)_context;
        public CourseCategory GetById(int id)
        {
            return Context.CourseCategories.Include(cc => cc.Courses).FirstOrDefault(cc => cc.Id == id);
        }

        public override List<CourseCategory> GetList()
        {
            return Context.CourseCategories.Include(cc => cc.Courses).ToList();
        }
    }
}
