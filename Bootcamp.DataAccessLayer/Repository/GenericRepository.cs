using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.DataAccessLayer.Repository
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        protected readonly Context _context;

        public GenericRepository(Context context)
        {
            _context = context;
        }

        public void Delete(T t)
        {
          _context.Remove(t);
            _context.SaveChanges();
        }

        public virtual T GetById(int id)
        {
         return _context.Set<T>().Find(id);
        }

        public virtual List<T> GetList()
        {
            return _context.Set<T>().ToList();
        }

        public void Insert(T t)
        {
           _context.Add(t);
            _context.SaveChanges();
        }

        public void Update(T t)
        {
            _context.Update(t);
            _context.SaveChanges();
        }
    }
}
