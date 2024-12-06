using SkillCrest_LearningPlatform.ViewModels.QuizViewModels;
using SkillCrest_LearningPlatform.ViewModels.StatisticsViewModels;

namespace SkillCrest_LearningPlatform.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<ICollection<CourseStatisticsViewModel>> GetCoureStatistic();
        Task<ICollection<StatisticsSubmissionViewModel>?> GetSubmissionsForLesson(string lessonId);
        Task<ICollection<QuizShortDetails>?> GetQuizSubmissions(string quizId);
    }
}
