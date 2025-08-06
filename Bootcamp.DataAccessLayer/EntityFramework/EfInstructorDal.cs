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
    public class EfInstructorDal: GenericRepository<Instructor>, IInstructorDal
    {
        public EfInstructorDal(Context context) : base(context)
        { }

        private Context Context => (Context)_context;
        public Instructor GetById(int id)
        {
            return Context.Instructors.Include(i => i.Courses).FirstOrDefault(i => i.Id == id);
        }

        public override List<Instructor> GetList()
        {
            return Context.Instructors.Include(i => i.Courses).ToList();
        }
    }
}
