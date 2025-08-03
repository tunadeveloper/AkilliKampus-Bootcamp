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
    public class CommentManager : ICommentService
    {
        private readonly ICommentDal _commentDal;

        public CommentManager(ICommentDal commentDal)
        {
            _commentDal = commentDal;
        }

        public void DeleteBL(Comment t)
        {
            _commentDal.Delete(t);
        }

        public Comment GetByIdBL(int id)
        {
            return _commentDal.GetById(id);
        }

        public List<Comment> GetListBL()
        {
            return _commentDal.GetList();
        }

        public void InsertBL(Comment t)
        {
         _commentDal.Insert(t);
        }

        public void UpdateBL(Comment t)
        {
         _commentDal.Update(t);
        }
    }
}
