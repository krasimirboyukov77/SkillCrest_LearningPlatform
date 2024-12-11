using Microsoft.Extensions.DependencyInjection;

using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;
using SkillCrest_LearningPlatform.Infrastructure.Repositories;
using SkillCrest_LearningPlatform.Services;
using SkillCrest_LearningPlatform.Services.Interfaces;

namespace SkillCrest_LearningPlatform.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection services)
        {
            // Add repositories
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

            // Add services
            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<IManageService, ManageService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
