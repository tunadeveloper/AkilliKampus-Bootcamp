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
    public class InstructorManager : IInstructorService
    {
        private readonly IInstructorDal _instructorDal;

        public InstructorManager(IInstructorDal instructorDal)
        {
            _instructorDal = instructorDal;
        }

        public void DeleteBL(Instructor t)
        {
           _instructorDal.Delete(t);
        }

        public Instructor GetByIdBL(int id)
        {
          return _instructorDal.GetById(id);
        }

        public List<Instructor> GetListBL()
        {
            return _instructorDal.GetList();
        }

        public void InsertBL(Instructor t)
        {
          _instructorDal.Insert(t);
        }

        public void UpdateBL(Instructor t)
        {
          _instructorDal.Update(t);
        }
    }
}
