using SkillCrest_LearningPlatform.ViewModels.StatisticsViewModels;

namespace SkillCrest_LearningPlatform.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<ICollection<CourseStatisticsViewModel>> GetCoureStatistic();
    }
}
