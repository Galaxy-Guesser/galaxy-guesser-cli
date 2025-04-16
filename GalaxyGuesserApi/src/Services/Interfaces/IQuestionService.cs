using GalaxyGuesserApi.Models;

namespace GalaxyGuesserApi.Services.Interfaces
{
     public interface IQuestionService
    {
        Task<QuestionResponse> GetNextQuestionForSessionAsync(int sessionId);
        Task<List<OptionResponse>> GetOptionsByQuestionIdAsync(int questionId);
        Task<int> GetQuestionCountForCategory(int categoryId);
        Task<AnswerResponse> GetCorrectAnswerAsync(int questionId);
    }
}