using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.BusinessLayer.Concrete
{
    public class ReferenceManager : IReferenceService
    {
        private readonly IReferenceDal _referenceDal;

        public ReferenceManager(IReferenceDal referenceDal)
        {
            _referenceDal = referenceDal;
        }

        public void InsertBL(Reference entity)
        {
            _referenceDal.Insert(entity);
        }

        public void UpdateBL(Reference entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _referenceDal.Update(entity);
        }

        public void DeleteBL(Reference entity)
        {
            _referenceDal.Delete(entity);
        }

        public List<Reference> GetListBL()
        {
            return _referenceDal.GetList();
        }

        public Reference GetByIdBL(int id)
        {
            return _referenceDal.GetById(id);
        }

        public List<Reference> GetActiveReferences()
        {
            return _referenceDal.GetList().Where(r => r.IsActive).ToList();
        }
    }
} 