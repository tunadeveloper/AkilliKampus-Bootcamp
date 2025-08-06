using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.BusinessLayer.Concrete
{
    public class SiteSettingManager : ISiteSettingService
    {
        private readonly ISiteSettingDal _siteSettingDal;

        public SiteSettingManager(ISiteSettingDal siteSettingDal)
        {
            _siteSettingDal = siteSettingDal;
        }

        public void InsertBL(SiteSetting entity)
        {
            _siteSettingDal.Insert(entity);
        }

        public void UpdateBL(SiteSetting entity)
        {
            entity.UpdatedAt = DateTime.Now;
            _siteSettingDal.Update(entity);
        }

        public void DeleteBL(SiteSetting entity)
        {
            _siteSettingDal.Delete(entity);
        }

        public List<SiteSetting> GetListBL()
        {
            return _siteSettingDal.GetList();
        }

        public SiteSetting GetByIdBL(int id)
        {
            return _siteSettingDal.GetById(id);
        }

        public List<SiteSetting> GetByGroup(string group)
        {
            return _siteSettingDal.GetByGroup(group);
        }

        public SiteSetting GetByKey(string key)
        {
            return _siteSettingDal.GetByKey(key);
        }

        public string GetValueByKey(string key)
        {
            var setting = _siteSettingDal.GetByKey(key);
            return setting?.Value ?? string.Empty;
        }

        public void UpdateSetting(string key, string value)
        {
            var setting = _siteSettingDal.GetByKey(key);
            if (setting != null)
            {
                setting.Value = value;
                setting.UpdatedAt = DateTime.Now;
                _siteSettingDal.Update(setting);
            }
        }
    }
} 