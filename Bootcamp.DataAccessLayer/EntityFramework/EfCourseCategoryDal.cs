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
    public class EfCourseCategoryDal : GenericRepository<CourseCategory>, ICourseCategoryDal
    {
        public EfCourseCategoryDal(Context context) : base(context)
        {
        }
    }
}
