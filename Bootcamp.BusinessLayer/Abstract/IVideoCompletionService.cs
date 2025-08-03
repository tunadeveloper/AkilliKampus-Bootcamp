using Bootcamp.EntityLayer.Concrete;

namespace Bootcamp.BusinessLayer.Abstract
{
    public interface IVideoCompletionService : IGenericService<VideoCompletion>
    {
        List<VideoCompletion> GetUserVideoCompletions(int userId, int courseId);
        VideoCompletion GetUserVideoCompletion(int userId, int courseId, int videoId);
        void MarkVideoAsCompleted(int userId, int courseId, int videoId);
        void MarkVideoAsIncomplete(int userId, int courseId, int videoId);
    }
} 