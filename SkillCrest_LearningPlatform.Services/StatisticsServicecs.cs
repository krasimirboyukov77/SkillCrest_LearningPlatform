using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;
using SkillCrest_LearningPlatform.Services.Interfaces;
using SkillCrest_LearningPlatform.ViewModels.StatisticsViewModels;
using Microsoft.EntityFrameworkCore;
using SkillCrest_LearningPlatform.Data.Migrations;
using SkillCrest_LearningPlatform.Data.Models;
using SkillCrest_LearningPlatform.Data.Models.QuizModels;
using SkillCrest_LearningPlatform.ViewModels.QuizViewModels;


namespace SkillCrest_LearningPlatform.Services
{
    public class StatisticsService : BaseService, IStatisticsService
    {
        private readonly IRepository<UserLessonProgress> _userLessonProgressRepository;
        private readonly IRepository<UserCourse> _userCoursesRepository;
        private readonly IRepository<Lesson> _lessonsRepository;
        private readonly IRepository<Submission> _submissionRepository;
        private readonly IRepository<QuizSubmission> _quizSubmissionRepository;

        public StatisticsService(
            IRepository<UserLessonProgress> userLessonProgress,
            IRepository<UserCourse> userCourse,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Submission> submissionRepository,
            IRepository<Lesson> lessonsRepository,
            IRepository<QuizSubmission> quizSubmissionRepository,
            UserManager<ApplicationUser> userManager)
            :base(httpContextAccessor, userManager)
        {
            this._userLessonProgressRepository = userLessonProgress;
            this._userCoursesRepository = userCourse;
            this._lessonsRepository = lessonsRepository;
            this._submissionRepository = submissionRepository;
            this._quizSubmissionRepository = quizSubmissionRepository;
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

        public async Task<ICollection<StatisticsSubmissionViewModel>?> GetSubmissionsForLesson(string lessonId)
        {
            Guid lessonGuid = Guid.Empty;
            bool isValidGuid = IsGuidValid(lessonId, ref lessonGuid);

            if (!isValidGuid)
            {
                return null;
            }

            var lessonSubmissions = await _submissionRepository
                .GetAllAttached()
                .Include(s=> s.Uploader)
                .Where(s=> s.LessonId == lessonGuid)
                .Select(s=> new StatisticsSubmissionViewModel()
                {
                    FileName = s.FileName,
                    FilePath = s.FilePath,
                    UploaderName = s.Uploader.FullName,
                    Id = s.Id.ToString()
                })
                .ToListAsync();

            return lessonSubmissions;
        }

        public async Task<ICollection<QuizShortDetails>?> GetQuizSubmissions(string quizId)
        {
            Guid quizGuid = Guid.Empty;
            bool isValidGuid = IsGuidValid(quizId, ref quizGuid);

            if (!isValidGuid)
            {
                return null;
            }

            var viewModel = await _quizSubmissionRepository
                .GetAllAttached()
                .Include(s=> s.Quiz)
                .Include(s=> s.Student)
                .Where(qs=> qs.QuizId == quizGuid)
                .Select(qs=> new QuizShortDetails()
                {
                    Id = qs.Id,
                    Title = qs.Quiz.Title,
                    TotalScore = qs.TotalScore,
                    Score = qs.Score,
                    Submitter = qs.Student.FullName
                })
                .ToListAsync();

            return viewModel;
        }
    }
}
