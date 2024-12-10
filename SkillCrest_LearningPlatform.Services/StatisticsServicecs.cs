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
        private readonly IRepository<Grade> _gradeRepository;

        public StatisticsService(
            IRepository<UserLessonProgress> userLessonProgress,
            IRepository<UserCourse> userCourse,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Submission> submissionRepository,
            IRepository<Lesson> lessonsRepository,
            IRepository<QuizSubmission> quizSubmissionRepository,
            UserManager<ApplicationUser> userManager,
            IRepository<Grade> gradeRepository)
            :base(httpContextAccessor, userManager)
        {
            this._userLessonProgressRepository = userLessonProgress;
            this._userCoursesRepository = userCourse;
            this._lessonsRepository = lessonsRepository;
            this._submissionRepository = submissionRepository;
            this._quizSubmissionRepository = quizSubmissionRepository;
            this._gradeRepository = gradeRepository;
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
                    Id = s.Id.ToString(),
                    FileName = s.FileName,
                    FilePath = s.FilePath,
                    UploaderName = s.Uploader.FullName,
                    UploaderId = s.UploaderId.ToString()
                })
                .ToListAsync();

            foreach (var submission in lessonSubmissions)
            {
                var grade = await _gradeRepository.FirstOrDefaultAsync(g => g.SubmissionId.ToString() == submission.Id);

                if (grade != null)
                {
                    GradeViewModel gradeModel = new GradeViewModel()
                    {
                        Id = grade.Id.ToString(),
                        Grade = grade.Score,
                        Comment = grade.Comment,
                    };

                    submission.Grade = gradeModel;
                }
            }

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

        public async Task<bool> Evaluate(CreateGradeViewModel viewModel)
        {
            Guid submissionGuid = Guid.Empty;
            bool isValidGuid = IsGuidValid(viewModel.SubmissionId, ref submissionGuid);

            if (!isValidGuid)
            {
                return false;
            }


            var submission = await _submissionRepository.FirstOrDefaultAsync(s => s.Id == submissionGuid);

            if (submission == null)
            {
                return false;
            }

            Grade newGrade = new Grade()
            {
                Comment = viewModel.Comment,
                SubmissionId = submissionGuid,
                Score = viewModel.Grade
            };

            await _gradeRepository.AddAsync(newGrade);

            return true;
        }
    }
}
