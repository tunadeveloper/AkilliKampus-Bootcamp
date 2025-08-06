using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.BusinessLayer.Abstract
{
    public interface IReferenceService : IGenericService<Reference>
    {
        List<Reference> GetActiveReferences();
    }
} 