using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.BusinessLayer.Concrete
{
    public class CourseVideoManager : ICourseVideoService
    {
        private readonly ICourseVideoDal _courseVideoDal;

        public CourseVideoManager(ICourseVideoDal courseVideoDal)
        {
            _courseVideoDal = courseVideoDal;
        }

        public void DeleteBL(CourseVideo t)
        {
            _courseVideoDal.Delete(t);
        }

        public CourseVideo GetByIdBL(int id)
        {
            return _courseVideoDal.GetById(id);
        }

        public List<CourseVideo> GetListBL()
        {
            return _courseVideoDal.GetList();
        }

        public void InsertBL(CourseVideo t)
        {
            _courseVideoDal.Insert(t);
        }

        public void UpdateBL(CourseVideo t)
        {
            _courseVideoDal.Update(t);
        }
    }
} 