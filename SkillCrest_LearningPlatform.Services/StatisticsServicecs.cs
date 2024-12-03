using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;
using SkillCrest_LearningPlatform.Services.Interfaces;
using SkillCrest_LearningPlatform.ViewModels.StatisticsViewModels;
using Microsoft.EntityFrameworkCore;


namespace SkillCrest_LearningPlatform.Services
{
    public class StatisticsService : BaseService, IStatisticsService
    {
        private readonly IRepository<UserLessonProgress> _userLessonProgressRepository;
        private readonly IRepository<UserCourse> _userCoursesRepository;
        private readonly IRepository<Lesson> _lessonsRepository;

        public StatisticsService(
            IRepository<UserLessonProgress> userLessonProgress,
            IRepository<UserCourse> userCourse,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Lesson> lessonsRepository,
            UserManager<ApplicationUser> userManager)
            :base(httpContextAccessor, userManager)
        {
            this._userLessonProgressRepository = userLessonProgress;
            this._userCoursesRepository = userCourse;
            this._lessonsRepository = lessonsRepository;
        }

        public async Task<ICollection<CourseStatisticsViewModel>> GetCoureStatistic()
        {
            var userCourses = await _userCoursesRepository.GetAllAttached()
                .Include(uc=> uc.Course)
                .Where(uc=> uc.UserId == GetUserId())
                .ToListAsync();

            var courseList = new List<CourseStatisticsViewModel>();

            foreach (var c in userCourses)
            {
                var totalLessons = await _lessonsRepository.GetAllAttached().Where(l=> l.CourseId == c.CourseId).ToListAsync();

                var completedLessonsForUser = 0;

                foreach(var l in totalLessons)
                {
                    var isLessonCompelted = await _userLessonProgressRepository.FirstOrDefaultAsync(ulp => ulp.LessonId == l.Id && ulp.UserId == GetUserId());
                    if(isLessonCompelted != null)
                    {
                        completedLessonsForUser++;
                    }
                }

                double completedPercentage;

                if (completedLessonsForUser == 0 || totalLessons.Count == 0)
                {
                    completedPercentage = 0;
                }
                else
                {
                    completedPercentage = ((double)completedLessonsForUser / (double)totalLessons.Count)*100;
                }

                CourseStatisticsViewModel statistics = new()
                {
                    CourseName = c.Course.Title,
                    TotalLessons = totalLessons.Count,
                    CompletedLessons = completedLessonsForUser,
                    CompletedPercentage = completedPercentage
                };
                courseList.Add(statistics);
            }

            return courseList;
        }
    }
}
