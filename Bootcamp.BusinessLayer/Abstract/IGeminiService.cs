using System.Threading.Tasks;

namespace Bootcamp.BusinessLayer.Abstract
{
    public interface IGeminiService
    {
        Task<string> SummarizeVideoAsync(string videoUrl, string videoTitle, string videoDescription);
        Task<string> GenerateVideoSummaryAsync(string videoContent);
        Task<string> AskQuestionAsync(string question, string videoTitle, string videoDescription);
    }
} 