using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.DataAccessLayer.Abstract
{
    public interface ISiteSettingDal : IGenericDal<SiteSetting>
    {
        List<SiteSetting> GetByGroup(string group);
        SiteSetting GetByKey(string key);
    }
} 