using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;
using SkillCrest_LearningPlatform.Services.Interfaces;
using SkillCrest_LearningPlatform.ViewModels.ManageViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace SkillCrest_LearningPlatform.Services
{
    public class ManageService : BaseService, IManageService
    {
        private readonly IRepository<Course> _courseRepository;

        public ManageService(IRepository<Course> courseRepository,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
            :base(httpContextAccessor, userManager)
        {
            this._courseRepository = courseRepository;
        }

        public async Task<IEnumerable<CourseManageViewModel>?> GetCoursesForManage()
        {
            var courses = await  _courseRepository.GetAllAttached()
                .Include(c=> c.Creator)
                .Select(c=> new CourseManageViewModel()
                {
                    Id = c.Id,
                    Title = c.Title,
                    DateCreated = c.DateCreated.ToString(),
                    CreatorName = c.Creator.UserName ?? string.Empty
                })
                .ToListAsync();

            return courses;
        }

        public async Task<CourseShortDetails?> GetLessonsForCourseToManage(string courseId)
        {
            Guid courseGuid = Guid.Empty;
            bool isValidGuid = IsGuidValid(courseId, ref courseGuid);

            if (!isValidGuid)
            {
                return null;
            }

            var course = await _courseRepository
                .GetAllAttached()
                .Include(c => c.Lessons)
                .Include(c => c.Creator)
                .FirstOrDefaultAsync(c => c.Id == courseGuid);

            if (course == null)
            {
                return null;
            }

            var courseWithLessons = new CourseShortDetails()
            {
                CourseId = courseGuid,
                CourseTitle = course.Title,
                Lessons = course.Lessons.Select(c => new LessonManageViewModel()
                {
                    Id = c.Id,
                    Title = c.Title,
                    DateCreated = c.DateCreated.ToString(),
                    DueDate = c.DueDate.ToString()
                })
                .ToList()
            };

            return courseWithLessons;
        }
    }
}
