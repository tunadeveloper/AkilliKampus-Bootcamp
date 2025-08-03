using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.BusinessLayer.Concrete
{
    public class VideoCompletionManager : IVideoCompletionService
    {
        private readonly IVideoCompletionDal _videoCompletionDal;

        public VideoCompletionManager(IVideoCompletionDal videoCompletionDal)
        {
            _videoCompletionDal = videoCompletionDal;
        }

        public void DeleteBL(VideoCompletion t)
        {
            _videoCompletionDal.Delete(t);
        }

        public VideoCompletion GetByIdBL(int id)
        {
            return _videoCompletionDal.GetById(id);
        }

        public List<VideoCompletion> GetListBL()
        {
            return _videoCompletionDal.GetList();
        }

        public void InsertBL(VideoCompletion t)
        {
            _videoCompletionDal.Insert(t);
        }

        public void UpdateBL(VideoCompletion t)
        {
            _videoCompletionDal.Update(t);
        }

        public List<VideoCompletion> GetUserVideoCompletions(int userId, int courseId)
        {
            return _videoCompletionDal.GetList()
                .Where(vc => vc.UserId == userId && vc.CourseId == courseId)
                .ToList();
        }

        public VideoCompletion GetUserVideoCompletion(int userId, int courseId, int videoId)
        {
            return _videoCompletionDal.GetList()
                .FirstOrDefault(vc => vc.UserId == userId && vc.CourseId == courseId && vc.CourseVideoId == videoId);
        }

        public void MarkVideoAsCompleted(int userId, int courseId, int videoId)
        {
            var existingCompletion = GetUserVideoCompletion(userId, courseId, videoId);
            
            if (existingCompletion == null)
            {
                // Yeni tamamlanma kaydı oluştur
                var newCompletion = new VideoCompletion
                {
                    UserId = userId,
                    CourseId = courseId,
                    CourseVideoId = videoId,
                    IsCompleted = true,
                    CompletedAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                };
                InsertBL(newCompletion);
            }
            else
            {
                // Mevcut kaydı güncelle
                existingCompletion.IsCompleted = true;
                existingCompletion.CompletedAt = DateTime.Now;
                existingCompletion.UpdatedAt = DateTime.Now;
                UpdateBL(existingCompletion);
            }
        }

        public void MarkVideoAsIncomplete(int userId, int courseId, int videoId)
        {
            var existingCompletion = GetUserVideoCompletion(userId, courseId, videoId);
            
            if (existingCompletion != null)
            {
                existingCompletion.IsCompleted = false;
                existingCompletion.CompletedAt = null;
                existingCompletion.UpdatedAt = DateTime.Now;
                UpdateBL(existingCompletion);
            }
        }
    }
} 