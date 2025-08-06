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
    public class EfCommentDal : GenericRepository<Comment>, ICommentDal
    {
        public EfCommentDal(Context context) : base(context)
        {
        }

        public override List<Comment> GetList()
        {
            return _context.Comments
                .Include(c => c.ApplicationUser)
                .Include(c => c.Course)
                .ToList();
        }

        public override Comment GetById(int id)
        {
            return _context.Comments
                .Include(c => c.ApplicationUser)
                .Include(c => c.Course)
                .FirstOrDefault(c => c.Id == id);
        }
    }
}
