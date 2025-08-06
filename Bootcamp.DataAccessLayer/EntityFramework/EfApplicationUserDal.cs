using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.DataAccessLayer.Concrete;
using Bootcamp.DataAccessLayer.Repository;
using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.DataAccessLayer.EntityFramework
{
    public class EfApplicationUserDal : GenericRepository<ApplicationUser>, IApplicationUserDal
    {
        public EfApplicationUserDal(Context context) : base(context)
        {
        }
    }
} 