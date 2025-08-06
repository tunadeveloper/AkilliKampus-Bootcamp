using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.BusinessLayer.Abstract
{
    public interface ISiteSettingService : IGenericService<SiteSetting>
    {
        List<SiteSetting> GetByGroup(string group);
        SiteSetting GetByKey(string key);
        string GetValueByKey(string key);
        void UpdateSetting(string key, string value);
    }
} 