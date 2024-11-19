using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;
using SkillCrest_LearningPlatform.Services.Interfaces;
using SkillCrest_LearningPlatform.ViewModels.CourseViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using SkillCrest_LearningPlatform.ViewModels.LessonViewModels;
using System.Globalization;
using SkillCrest_LearningPlatform.Data.Models;

namespace SkillCrest_LearningPlatform.Services
{
    public class CourseService : BaseService,ICourseService
    {

        private readonly IRepository<Course> _repository;
        private readonly IRepository<UserCourse> _userCourseRepository;


        public CourseService(IRepository<Course> repository, 
            IHttpContextAccessor httpContextAccessor,
            IRepository<UserCourse> userCourseRepository,
            IRepository<Manager> managerRepository) 
            : base(httpContextAccessor, managerRepository) 
        
        {
            this._repository = repository;
            this._userCourseRepository = userCourseRepository;
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

        public async Task<CourseDetailsViewModel?> GetDetailsAboutCourseAsync(string id)
        {
            Guid courseGuid = Guid.Empty;
            bool isValidGuid = IsGuidValid(id, ref courseGuid);

            if (!isValidGuid)
            {
                return null;
            }

            var course = await _repository
                .GetAllAttached()
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
                    })
                    .OrderBy(ls=> ls.IsCompleted)
                    .ToList(),
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
                
            };

            bool isValidImage = isValidUrl(viewModel.ImageUrl ?? string.Empty);

            if (isValidImage)
            {
                course.ImageUrl = viewModel.ImageUrl;
            }
            else
            {
                course.ImageUrl = "wwwroot/images/course-default-image.jpg";
            }

            await _repository.AddAsync(course);
        }

        public async Task<CourseEditViewModel?> GetCourseForEditAsync(string id)
        {
            Guid courseGuid = Guid.Empty;
            bool isValidGuid = IsGuidValid(id, ref courseGuid);

            if (!isValidGuid)
            {
                return null;
            }

            var course = await _repository
                .GetAllAttached()
                .FirstOrDefaultAsync(c => c.Id == courseGuid);
  
            if (course != null)
            {
                CourseEditViewModel viewModel = new()
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    ImageUrl = course.ImageUrl,
                    DateCreated = course.DateCreated.ToString("dd-MM-yyyy"),
                };
                return viewModel;
            }

            return null;
        }

        public async Task<bool> EditCourse(CourseEditViewModel viewModel)
        {
            Course? course = await _repository
                .GetAllAttached().
                FirstOrDefaultAsync(c => c.Id == viewModel.Id);

            bool isValidDate = DateTime.TryParseExact(viewModel.DateCreated, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var courseDateCreated);

            if (course != null)
            {
                course.Title = viewModel.Title;
                course.Description = viewModel.Description;
                course.ImageUrl = viewModel.ImageUrl;

                var isEditsuccessful = await _repository.UpdateAsync(course);
                if (isEditsuccessful)
                {
                    return true;
                }

            }

            return false;
        }

        public async Task<bool> EnrollStudent(string courseId)
        {
            Guid courseGuid = Guid.Empty;
            bool isValidGuid = IsGuidValid(courseId, ref courseGuid);

            if (!isValidGuid)
            {
                return false;
            }

            var userId = GetUserId();

            if (userId == Guid.Empty)
            {
                return false;
            }

            var isUserAdded = await _userCourseRepository.FirstOrDefaultAsync(uc=> uc.UserId == userId && uc.CourseId == courseGuid);


            if (isUserAdded == null)
            {
                UserCourse enrolledUser = new()
                {
                    UserId = userId,
                    CourseId = courseGuid,
                };

                await _userCourseRepository.AddAsync(enrolledUser);
            }

            return true;
        }

        public async Task<IEnumerable<UserShortDetailsViewModel>?> GetUsersEnrolled(string courseId)
        {
            Guid courseGuid = Guid.Empty;
            bool isValidGuid = IsGuidValid(courseId, ref courseGuid);

            if (!isValidGuid)
            {
                return null;
            }

            var usersInCourse = await _userCourseRepository
                .GetAllAttached()
                .AsNoTracking()
                .Include(uc=> uc.User)
                .Where(uc => uc.CourseId == courseGuid)
                .Select(uc=> new UserShortDetailsViewModel()
                {
                    UserName = uc.User.UserName ?? string.Empty,
                    ImageUrl = uc.User.UserImage ?? string.Empty,
                })
                .ToListAsync();

            if (usersInCourse.Any())
            {
                return usersInCourse;
            }

            return null;
        }

        private bool isValidUrl(string url)
        {
            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
