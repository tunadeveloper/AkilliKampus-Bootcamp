using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.DataAccessLayer.Concrete;
using Bootcamp.DataAccessLayer.Repository;
using Bootcamp.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp.DataAccessLayer.EntityFramework
{
    public class EfReferenceDal : GenericRepository<Reference>, IReferenceDal
    {
        public EfReferenceDal(Context context) : base(context)
        {
        }

        public override List<Reference> GetList()
        {
            return _context.References
                .OrderBy(r => r.Order)
                .ThenBy(r => r.CreatedAt)
                .ToList();
        }
    }
} 