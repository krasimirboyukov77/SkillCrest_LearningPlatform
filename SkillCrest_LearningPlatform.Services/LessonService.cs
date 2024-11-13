using Microsoft.AspNetCore.Http;
using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.Infrastructure.Repositories;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;
using SkillCrest_LearningPlatform.Services.Interfaces;
using SkillCrest_LearningPlatform.ViewModels.LessonViewModels;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNet.Identity;


namespace SkillCrest_LearningPlatform.Services
{
    public class LessonService : BaseService, ILessonService
    {
        private readonly IRepository<Lesson> _lessonRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<UserLessonProgress> _userLessonProgressRepository;
        
        public LessonService(IRepository<Lesson> lessonRepository, 
            IRepository<Course> courseRepository,
            IRepository<UserLessonProgress> userLessonProgressRepository,
            IHttpContextAccessor httpContextAccessor )
            :base(httpContextAccessor)
        {
            this._courseRepository = courseRepository;
            this._lessonRepository = lessonRepository;
            this._userLessonProgressRepository = userLessonProgressRepository;
        }
        public async Task<bool> CreateLesson(CreateLessonViewModel viewModel, IFormFile file)
        {
            var filePathEntity = "";
            var fileName = "";
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", file.FileName);
                filePathEntity = filePath;
                fileName = Path.GetFileName(filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

            }


            bool isValidDate = DateTime.TryParseExact(viewModel.DueDate, Common.Lesson.ValidationConstants.LessonDateCreatedFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime lessonDueDate);

            if (!isValidDate)
            {
                return false;
            }

            Guid courseId = Guid.Empty;
            bool isValidCourseId = IsGuidValid(viewModel.CourseId,ref courseId);

            if (!isValidCourseId)
            {
                return false;
            }

            Course? course = await _courseRepository.GetByIdAsync(courseId);

            if (course == null)
            {
                return false;
            }

            Lesson lesson = new()
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                DueDate = lessonDueDate,
                DateCreated = DateTime.Now,
                CourseId = courseId,
                CreatorId = GetUserId(),
                FilePath = filePathEntity,
                FileName = fileName
            };

            if(viewModel.Points > 0)
            {
                lesson.Points = viewModel.Points;
            }
            else
            {
                lesson.Points = 0;
            }

            course.Lessons.Add(lesson);

            await _lessonRepository.AddAsync(lesson);
            return true;
        }

        public async Task<LessonDetailsViewModel?>? GetLessonDetails(string id)
        {

            Guid lessonGuid = Guid.Empty;
            bool isValidGuid = IsGuidValid(id, ref lessonGuid);

            if (!isValidGuid)
            {
                return null;
            }

            var lesson = await _lessonRepository
                .GetAllAttached()
                .Include(l => l.Creator)
                .FirstOrDefaultAsync(l => l.Id == lessonGuid);

            LessonDetailsViewModel? viewModel = null;

            if (lesson != null)
            {
                viewModel = new()
                {
                    Id = lesson.Id,
                    Title = lesson.Title,
                    Description = lesson.Description ?? string.Empty,
                    Creator = lesson.Creator.UserName ?? string.Empty,
                    DueDate = lesson.DueDate.ToString(Common.Lesson.ValidationConstants.LessonDateCreatedFormat),
                    DateCreated = lesson.DateCreated.ToString(Common.Lesson.ValidationConstants.LessonDateCreatedFormat),
                    Points = lesson.Points,
                    FilePath = lesson.FilePath,
                    FileName = lesson.FileName
                };

            }

            return viewModel;
        }

        public async Task<bool> ToggleLessonCompletion(string lessonId, string courseId)
        {
            Guid lessonGuid = Guid.Empty;
            bool isValidLessonId = IsGuidValid(lessonId, ref lessonGuid);

            if (!isValidLessonId)
            {
                return false;
            }

            Guid courseGuid = Guid.Empty;
            bool isValidCourseId = IsGuidValid(courseId, ref courseGuid);

            if (!isValidCourseId)
            {
                return false;
            }


            Lesson? lesson = await _lessonRepository.GetByIdAsync(lessonGuid);

            if (lesson == null)
            {
                return false;
            }
            
            bool UserLessonProgerssExists = await _userLessonProgressRepository.Any(ulp => ulp.LessonId == lessonGuid && ulp.UserId == GetUserId());

            if (UserLessonProgerssExists)
            {
                return false;
            }

            UserLessonProgress userLessonProgress = new()
            {
                LessonId = lessonGuid,
                UserId = GetUserId(),
                IsCompleted = true,
                CompletionDate = DateTime.Now,
            };

            await _userLessonProgressRepository.AddAsync(userLessonProgress);  

            return true;
        }

        public async Task<bool> MarkAsIncomplete(string lessonId, string courseId)
        {
            Guid lessonGuid = Guid.Empty;
            bool isValidLessonId = IsGuidValid(lessonId, ref lessonGuid);

            if (!isValidLessonId)
            {
                return false;
            }

            Guid courseGuid = Guid.Empty;
            bool isValidCourseId = IsGuidValid(courseId, ref courseGuid);

            if (!isValidCourseId)
            {
                return false;
            }

            Lesson? lesson = await _lessonRepository.GetByIdAsync(lessonGuid);

            if (lesson == null)
            {
                return false;
            }

            UserLessonProgress? UserLessonProgerssExists = await _userLessonProgressRepository.FirstOrDefaultAsync(ulp => ulp.LessonId == lessonGuid && ulp.UserId == GetUserId());

            if (UserLessonProgerssExists != null)
            {
                await _userLessonProgressRepository.DeleteEntityAsync(UserLessonProgerssExists);

            }

            return true;

        }

        public async Task<LessonDetailsViewModel?> GetLessonById(string lessonId)
        {
            Guid lessonGuid = Guid.Empty;

            bool islessonGuidValid = IsGuidValid(lessonId, ref lessonGuid);

            if (!islessonGuidValid) 
            {
                return null;
            }

            var lesson = await _lessonRepository
                .GetAllAttached()
                .Include(l => l.Creator)
                .FirstOrDefaultAsync(l => l.Id == lessonGuid);


            if(lesson == null)
            {
                return null;
            }


            var lessonDetails = new LessonDetailsViewModel()
            {
                Id = lessonGuid,
                Title = lesson.Title,
                Description = lesson.Description,
                DateCreated = lesson.DateCreated.ToString("dd-MM-yyyy"),
                DueDate = lesson.DueDate.ToString("dd-MM-yyyy"),
                Creator = lesson.Creator.UserName ?? string.Empty,
                Points = lesson.Points,
                FilePath = lesson.FilePath,
                FileName = lesson.FileName,
            };

            return lessonDetails;
        }

        public async Task<bool> EditLesson(LessonDetailsViewModel viewModel)
        {
            Lesson? lesson = await _lessonRepository
                .GetAllAttached().
                FirstOrDefaultAsync(l=> l.Id == viewModel.Id);

            bool isValidDate = DateTime.TryParseExact(viewModel.DueDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var lessonDueDate);

            if (lesson != null)
            {
                lesson.Title = viewModel.Title;
                lesson.Description = viewModel.Description;
                lesson.DueDate = lessonDueDate;

                var isEditsuccessful = await _lessonRepository.UpdateAsync(lesson);
                if(isEditsuccessful) 
                {
                    return true;
                }
                
            }

            return false;
        }

        public async Task<DeleteLessonViewModel?> GetLessonForDelete(string lessonId)
        {
            Guid lessonGuid = Guid.Empty;
            bool isValidId = IsGuidValid(lessonId, ref lessonGuid);

            if (!isValidId)
            {
                return null;
            }

            Lesson? lesson = await _lessonRepository
                .GetAllAttached()
                .FirstOrDefaultAsync(l => l.Id == lessonGuid);

            if (lesson == null)
            {
                return null;
            }

            DeleteLessonViewModel viewModel = new()
            {
                Id = lessonGuid,
                Title = lesson.Title,
                DateCreated = lesson.DateCreated.ToString("dd-MM-yyyy"),
                CreatorId = lesson.CreatorId,
                CourseId = lesson.CourseId,
            };

            return viewModel;   
        }

        public async Task<bool> DeleteLesson(DeleteLessonViewModel viewModel)
        {
            
            var userId = GetUserId();

            if(userId != viewModel.CreatorId)
            {
                return false;
            }

            Lesson? lesson = await _lessonRepository
                .FirstOrDefaultAsync(l => l.Id == viewModel.Id);

            if(lesson == null)
            {
                return false;
            }

            var isDeleteSuccessful = await _lessonRepository.DeleteEntityAsync(lesson);

            if (isDeleteSuccessful)
            {
                return true;
            }

            return false;
        }

    }
}

