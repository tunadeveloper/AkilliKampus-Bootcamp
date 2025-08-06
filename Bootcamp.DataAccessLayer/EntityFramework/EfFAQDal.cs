using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.DataAccessLayer.Concrete;
using Bootcamp.DataAccessLayer.Repository;
using Bootcamp.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp.DataAccessLayer.EntityFramework
{
    public class EfFAQDal : GenericRepository<FAQ>, IFAQDal
    {
        public EfFAQDal(Context context) : base(context)
        {
        }

        public override List<FAQ> GetList()
        {
            return _context.FAQs
                .OrderBy(f => f.Order)
                .ThenBy(f => f.CreatedAt)
                .ToList();
        }
    }
} 