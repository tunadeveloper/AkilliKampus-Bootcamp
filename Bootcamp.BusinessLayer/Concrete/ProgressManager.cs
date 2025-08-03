using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.BusinessLayer.Concrete
{
    public class ProgressManager : IProgressService
    {
        private readonly IProgressDal _progressDal;

        public ProgressManager(IProgressDal progressDal)
        {
            _progressDal = progressDal;
        }

        public void DeleteBL(Progress t)
        {
           _progressDal.Delete(t);
        }

        public Progress GetByIdBL(int id)
        {
           return _progressDal.GetById(id);
        }

        public List<Progress> GetListBL()
        {
        return _progressDal.GetList();
        }

        public void InsertBL(Progress t)
        {
          _progressDal.Insert(t);
        }

        public void UpdateBL(Progress t)
        {
            _progressDal.Update(t);
        }
    }
}
