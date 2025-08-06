using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.BusinessLayer.Concrete
{
    public class ApplicationUserManager : IApplicationUserService
    {
        private readonly IApplicationUserDal _userDal;

        public ApplicationUserManager(IApplicationUserDal userDal)
        {
            _userDal = userDal;
        }

        public void DeleteBL(ApplicationUser t)
        {
            _userDal.Delete(t);
        }

        public ApplicationUser GetByIdBL(int id)
        {
            return _userDal.GetById(id);
        }

        public List<ApplicationUser> GetListBL()
        {
            return _userDal.GetList();
        }

        public void InsertBL(ApplicationUser t)
        {
            _userDal.Insert(t);
        }

        public void UpdateBL(ApplicationUser t)
        {
            _userDal.Update(t);
        }
    }
} 