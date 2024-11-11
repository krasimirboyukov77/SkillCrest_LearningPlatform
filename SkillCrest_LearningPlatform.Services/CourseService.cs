using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;
using SkillCrest_LearningPlatform.Services.Interfaces;
using SkillCrest_LearningPlatform.ViewModels.CourseViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using SkillCrest_LearningPlatform.ViewModels.LessonViewModels;

namespace SkillCrest_LearningPlatform.Services
{
    public class CourseService : BaseService,ICourseService
    {

        private readonly IRepository<Course> _repository;

        public CourseService(IRepository<Course> repository, IHttpContextAccessor httpContextAccessor) 
            : base(httpContextAccessor) 
        
        {
            this._repository = repository;
        }

        public async Task<IEnumerable<CourseInfoViewModel>> IndexGetCoursesByDateAsync()
        {
            IEnumerable<CourseInfoViewModel> courseDetails = await this._repository
                .GetAllAttached()
                .AsNoTracking()
                .Include(c=> c.Creator)
                .Select(c => new CourseInfoViewModel()
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description ?? string.Empty,
                    DateCreated = c.DateCreated,
                    Creator = c.Creator.UserName ?? string.Empty


                })
                .OrderByDescending(c => c.DateCreated)
                .ToListAsync();

            return courseDetails;
        }

        public async Task<CourseDetailsViewModel?>? GetDetailsAboutCourseAsync(string id)
        {
            Guid courseGuid = Guid.Empty;
            bool isValidGuid = IsGuidValid(id, ref courseGuid);

            if (!isValidGuid)
            {
                return null;
            }

            var course = await _repository
                .GetAllAttached()
                .OrderBy(c => c.DateCreated)
                .Include(c => c.Lessons)
                .ThenInclude(c => c.UsersLessonsProgresses)
                .Include(c => c.Creator).FirstOrDefaultAsync(c => c.Id == courseGuid);
                

            CourseDetailsViewModel? viewModel = null;

            if (course != null)
            {
                viewModel = new()
                {
                    Id = course.Id,
                    Title = course.Title,
                    DateCreated = course.DateCreated.ToString("dd-MM-yyyy"),
                    CreatorId = course.CreatorId.ToString(),
                    Lessons = course.Lessons.Select(l => new LessonInCourseViewModel()
                    {
                        Id = l.Id,
                        Title = l.Title,
                        Description = l.Description,
                        DateCreated = l.DateCreated.ToString("dd-MM-yyyy"),
                        DueDate = l.DueDate.ToString("dd-MM-yyyy"),
                        Creator = l.Creator.UserName ?? string.Empty,
                        IsCompleted = l.UsersLessonsProgresses.Any(ul => ul.LessonId == l.Id && ul.UserId == GetUserId())
                    }).ToList(),
                };
            }

            return viewModel;

        }

        public async Task AddCourseAsync(CreateCourseViewModel viewModel)
        {

            Course course = new()
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                DateCreated = DateTime.Now,
                CreatorId = GetUserId(),
                ImageUrl = viewModel.ImageUrl
            };

            await _repository.AddAsync(course);
        }


    }
}
