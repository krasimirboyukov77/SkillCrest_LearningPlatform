using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.AspNetCore.Identity;

using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;
using SkillCrest_LearningPlatform.Services.Interfaces;
using SkillCrest_LearningPlatform.ViewModels.CourseViewModels;
using SkillCrest_LearningPlatform.Data.Models.QuizModels;
using SkillCrest_LearningPlatform.ViewModels.LessonViewModels;
using SkillCrest_LearningPlatform.Common;
using SkillCrest_LearningPlatform.ViewModels.QuizViewModels;

namespace SkillCrest_LearningPlatform.Services
{
    public class CourseService : BaseService,ICourseService
    {
        public const string CourseDateFormat = Common.Course.ValidationConstants.CourseDateCreatedFormat;
        public const string LessonDateFormat = Common.Lesson.ValidationConstants.LessonDateFormat;

        private readonly IRepository<Course> _repository;
        private readonly IRepository<UserCourse> _userCourseRepository;
        private readonly IRepository<Quiz> _quizRepository;
        private readonly IRepository<Lesson> _lessonRepository;
        private readonly IRepository<QuizSubmission> _quizSubmissionRepository;
        public CourseService(IRepository<Course> repository, 
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            IRepository<UserCourse> userCourseRepository,
            IRepository<Quiz> quizRepository,
            IRepository<Lesson> lessonRepository,
            IRepository<QuizSubmission> quizSubmissionRepository) 
            : base(httpContextAccessor, userManager) 
        
        {
           this._repository = repository;
           this._userCourseRepository = userCourseRepository;
           this._quizRepository = quizRepository;
           this._lessonRepository = lessonRepository;
           this._quizSubmissionRepository = quizSubmissionRepository;
        }
         
        public async Task<ICollection<CourseInfoViewModel>> GetCoursesSortedByDate(string? searchTerm)
        {
            var entities = string.IsNullOrEmpty(searchTerm)
       ? await _repository.GetAllAttached().AsNoTracking().Include(c=> c.Creator).ToListAsync()
        : await _repository.GetAllAttached()
        .AsNoTracking()
        .Include(c=> c.Creator)
        .Where(c =>
            c.Title.ToLower().Contains(searchTerm.ToLower()))
        .ToListAsync();

            var courseDetails = entities
                .Select(c => new CourseInfoViewModel()
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description ?? string.Empty,
                    DateCreated = c.DateCreated,
                    Creator = c.Creator.UserName ?? string.Empty,
                    Password = c.Password


                })
                .OrderBy(c => c.DateCreated)
                .ToList();

            return courseDetails;
        }


        public async Task<ICollection<CourseInfoViewModel>> IndexGetCoursesByDateAsync(string searchTerm)
        {
            ICollection<CourseInfoViewModel> courseDetails = await GetCoursesSortedByDate(searchTerm);

            var userId = GetUserId();

            foreach (var course in courseDetails)
            {
                var userEnrolled = await _userCourseRepository.GetAllAttached().Where(c => c.UserId == userId && c.CourseId == course.Id).FirstOrDefaultAsync();
                if (userEnrolled != null)
                {
                    course.IsEnrolled = true;
                }
                else
                {
                    course.IsEnrolled = false;
                }
            }

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
                
            var isEnrolled = await _userCourseRepository.FirstOrDefaultAsync(c=> c.UserId == GetUserId() && c.CourseId == courseGuid);
            if (isEnrolled == null)
            {
                return null;
            }

            CourseDetailsViewModel? viewModel = null;

            if (course != null)
            {
                viewModel = new()
                {
                    Id = course.Id,
                    Title = course.Title,
                    DateCreated = course.DateCreated.ToString(CourseDateFormat),
                    CreatorId = course.CreatorId.ToString(),
                    Lessons = course.Lessons.Select(l => new LessonInCourseViewModel()
                    {
                        Id = l.Id,
                        Title = l.Title,
                        Description = l.Description,
                        DateCreated = l.DateCreated.ToString(CourseDateFormat),
                        DueDate = l.DueDate?.ToString(LessonDateFormat),
                        Creator = l.Creator.UserName ?? string.Empty,
                        IsCompleted = l.UsersLessonsProgresses.Any(ul => ul.LessonId == l.Id && ul.UserId == GetUserId())
                    })
                    .OrderBy(ls => ls.IsCompleted)
                    .ToList(),
                };

                var quizzes = _quizRepository.GetAllAttached().Where(q => q.CourseId == course.Id).Select(q => new ViewModels.QuizViewModels.QuizShortDetails()
                {
                    Id = q.Id,
                    Title = q.Title,
                })
                     .ToList();

                var quizzesListViewModel = new List<QuizShortDetails>();

                foreach(var quiz in quizzes)
                {
                    var isSubmitted = await _quizSubmissionRepository
                        .GetAllAttached()
                        .Where(qs => qs.QuizId == quiz.Id && qs.StudentId == GetUserId())
                        .FirstOrDefaultAsync();

                    var model = new QuizShortDetails()
                    {
                        Id = quiz.Id,
                        Title = quiz.Title,
                        IsSubmitted = false
                    };
                    if (isSubmitted == null)
                    {
                        quizzesListViewModel.Add(model);
                    }
                    else
                    {
                        model.IsSubmitted = true;
                        model.TotalScore = isSubmitted.TotalScore;
                        model.Score = isSubmitted.Score;
                        quizzesListViewModel.Add(model);
                    }
                }

                viewModel.Quizzes = quizzesListViewModel;
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
                Password = viewModel.Password,
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

            UserCourse enrolledUser = new()
            {
                UserId = GetUserId(),
                CourseId = course.Id,
            };

            await _userCourseRepository.AddAsync(enrolledUser);
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
        public async Task<bool> EnrollStudentNoPassword(string courseId)
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

            var isUserEnrolled = await _userCourseRepository.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CourseId == courseGuid);


            if (isUserEnrolled == null)
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
        public async Task<bool> EnrollStudentWithPassword(CoursePasswordViewModel viewModel)
        {
            Guid courseGuid = Guid.Empty;
            bool isValidGuid = IsGuidValid(viewModel.CourseId, ref courseGuid);

            if (!isValidGuid)
            {
                return false;
            }

            var userId = GetUserId();

            if (userId == Guid.Empty)
            {
                return false;
            }

            var coursePassword = await _repository.GetAllAttached()
                .Where(c => c.Id == courseGuid)
                .Select(c => c.Password)
                .FirstAsync();

            if(coursePassword != viewModel.Password && !string.IsNullOrEmpty(viewModel.Password))
            {
                return false;
            }

            var isUserEnrolled = await _userCourseRepository.FirstOrDefaultAsync(uc=> uc.UserId == userId && uc.CourseId == courseGuid);


            if (isUserEnrolled == null)
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

        public async Task<UserListViewModel?> GetUsersEnrolled(string courseId)
        {
            Guid courseGuid = Guid.Empty;
            bool isValidCourseGuid = IsGuidValid(courseId, ref courseGuid);

            if (!isValidCourseGuid)
            {
                return null;
            }
            
            var course = await _repository.FirstOrDefaultAsync(c=> c.Id == courseGuid);

            if (course == null)
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
                    Id = uc.UserId.ToString().ToLowerInvariant(),
                })
                .ToListAsync();

            var userList = new UserListViewModel()
            {
                CreatorId = course.CreatorId.ToString().ToLowerInvariant(),
                CurrentCourseId = courseId,
                ShortDetails = usersInCourse,
            };

                return userList;     
        }

        public async Task<bool> LeaveCourse(string courseId)
        {
            Guid courseGuid = Guid.Empty;
            bool isValidGuid = IsGuidValid(courseId, ref courseGuid);

            if (!isValidGuid)
            {
                return false;
            }

            var course = await _repository.FirstOrDefaultAsync(c=> c.Id == courseGuid);

            if (course == null)
            {
                return false;
            }

            var userCourse = await _userCourseRepository.FirstOrDefaultAsync(uc=> uc.CourseId == courseGuid && uc.UserId == GetUserId());

            if (userCourse == null)
            {
                return false;
            }

            await _userCourseRepository.DeleteEntityAsync(userCourse);

            return true;
        }

        private bool isValidUrl(string url)
        {
            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public async Task<bool> DeleteCourse(string courseId)
        {
            Guid courseGuid = Guid.Empty;
            bool isValidGuid = IsGuidValid(courseId, ref courseGuid);

            if (!isValidGuid)
            {
                return false;
            }


            Course? course = await _repository
                .FirstOrDefaultAsync(l => l.Id == courseGuid);

            if (course == null)
            {
                return false;
            }

            course.IsDeleted = true;

            var lessons = await _lessonRepository.GetAllAttached().Where(l => l.CourseId == courseGuid).ToListAsync();

            if (lessons.Any())
            {

                foreach (var lesson in lessons)
                {
                    lesson.IsDeleted = true;
                    var isLessonsDeletedSuccessful = await _lessonRepository.UpdateAsync(lesson);
                    if (!isLessonsDeletedSuccessful)
                    {
                        return false;
                    }
                }
            }

            var isCourseDeleteSuccessful = await _repository.UpdateAsync(course);
            if (!isCourseDeleteSuccessful)
            {
                return false;
            }

            return true;
        }
        public async Task<bool> RemoveUserFromCourse(string courseId, string userId)
        {
            Guid courseGuid = Guid.Empty;
            bool isValidGuid = IsGuidValid(courseId, ref courseGuid);

            if (!isValidGuid)
            {
                return false;
            }

            var course = await _repository.FirstOrDefaultAsync(c => c.Id == courseGuid);

            if (course == null)
            {
                return false;
            }

            var user = await GetUser(userId);

            if (user == null)
            {
                return false;
            }

            var userCourse = await _userCourseRepository.FirstOrDefaultAsync(uc=> uc.UserId == user.Id && uc.CourseId == courseGuid);

            if (userCourse == null)
            {
                return false;
            }

            await _userCourseRepository.DeleteEntityAsync(userCourse);

            return true;
        }
    }
}
