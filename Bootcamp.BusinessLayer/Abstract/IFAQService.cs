using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.BusinessLayer.Abstract
{
    public interface IFAQService : IGenericService<FAQ>
    {
        List<FAQ> GetActiveFAQs();
    }
} 