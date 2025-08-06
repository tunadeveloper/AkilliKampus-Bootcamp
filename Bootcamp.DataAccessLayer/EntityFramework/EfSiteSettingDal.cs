using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.DataAccessLayer.Concrete;
using Bootcamp.DataAccessLayer.Repository;
using Bootcamp.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp.DataAccessLayer.EntityFramework
{
    public class EfSiteSettingDal : GenericRepository<SiteSetting>, ISiteSettingDal
    {
        public EfSiteSettingDal(Context context) : base(context)
        {
        }

        public List<SiteSetting> GetByGroup(string group)
        {
            return _context.SiteSettings
                .Where(s => s.Group == group && s.IsActive)
                .OrderBy(s => s.Key)
                .ToList();
        }

        public SiteSetting GetByKey(string key)
        {
            return _context.SiteSettings
                .FirstOrDefault(s => s.Key == key && s.IsActive);
        }
    }
} 