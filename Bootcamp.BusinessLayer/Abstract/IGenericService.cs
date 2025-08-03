using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.BusinessLayer.Abstract
{
    public interface IGenericService<T> where T : class
    {
        void InsertBL(T t);
        void UpdateBL(T t);
        void DeleteBL(T t);
        List<T> GetListBL();
        T GetByIdBL(int id);
    }

}
