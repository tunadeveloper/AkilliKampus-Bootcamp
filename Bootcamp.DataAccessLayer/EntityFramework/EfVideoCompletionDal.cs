using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.DataAccessLayer.Concrete;
using Bootcamp.DataAccessLayer.Repository;
using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.DataAccessLayer.EntityFramework
{
    public class EfVideoCompletionDal : GenericRepository<VideoCompletion>, IVideoCompletionDal
    {
        public EfVideoCompletionDal(Context context) : base(context)
        {
        }
    }
} 