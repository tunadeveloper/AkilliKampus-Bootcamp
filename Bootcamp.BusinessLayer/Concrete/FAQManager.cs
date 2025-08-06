using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.BusinessLayer.Concrete
{
    public class FAQManager : IFAQService
    {
        private readonly IFAQDal _faqDal;

        public FAQManager(IFAQDal faqDal)
        {
            _faqDal = faqDal;
        }

        public void InsertBL(FAQ entity)
        {
            _faqDal.Insert(entity);
        }

        public void UpdateBL(FAQ entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _faqDal.Update(entity);
        }

        public void DeleteBL(FAQ entity)
        {
            _faqDal.Delete(entity);
        }

        public List<FAQ> GetListBL()
        {
            return _faqDal.GetList();
        }

        public FAQ GetByIdBL(int id)
        {
            return _faqDal.GetById(id);
        }

        public List<FAQ> GetActiveFAQs()
        {
            return _faqDal.GetList().Where(f => f.IsActive).ToList();
        }
    }
} 