using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCrest_LearningPlatform.Data;
using SkillCrest_LearningPlatform.ViewModels.LessonViewModels;
using System.Globalization;
using SkillCrest_LearningPlatform.Common.Lesson;
using SkillCrest_LearningPlatform.Data.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SkillCrest_LearningPlatform.Controllers
{
    [Authorize]
    public class LessonController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LessonController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var lesson = await _context.Lessons.Include(l=> l.Creator).FirstOrDefaultAsync(l=> l.Id.ToString() == id);

            if (lesson == null)
            {
                return NotFound();
            }

            LessonDetailsViewModel viewModel = new()
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Description = lesson.Description ?? string.Empty,
                Creator = lesson.Creator.UserName ?? string.Empty,
                DueDate = lesson.DueDate.ToString(Common.Lesson.ValidationConstants.LessonDateCreatedFormat),
                DateCreated = lesson.DateCreated.ToString(Common.Lesson.ValidationConstants.LessonDateCreatedFormat),

            };

            return View(viewModel);
        }

        [HttpGet]
       public IActionResult Create(string courseId)
        {

            CreateLessonViewModel viewModel = new CreateLessonViewModel()
            {
                CourseId = courseId
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLessonViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            bool isValidDate = DateTime.TryParseExact(viewModel.DueDate, Common.Lesson.ValidationConstants.LessonDateCreatedFormat,CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime lessonDueDate);

            if (!isValidDate)
            {
                return View(viewModel);
            }

            bool isValidCourseId = Guid.TryParse(viewModel.CourseId, out Guid courseId);

            if (!isValidCourseId)
            {
                return View(viewModel);
            }

            Course? course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                return NotFound();
            }

            Lesson lesson = new()
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                DueDate = lessonDueDate,
                DateCreated = DateTime.Now,
                CourseId = courseId,
                CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty
            };

            course.Lessons.Add(lesson);

            await _context.AddAsync(lesson);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new {id = lesson.Id});
        }

        [HttpPost]
        public async Task<IActionResult> ToggleLessonCompletion(string lessonId, string courseId)
        {

            bool isValidLessonId = Guid.TryParse(lessonId, out Guid validLessonId);

            if (!isValidLessonId)
            {
                return RedirectToAction("Index", "Course");
            }

            bool isValidCourseId = Guid.TryParse(courseId, out Guid validCourseId);

            if (!isValidCourseId)
            {
                return RedirectToAction("Index", "Course");
            }


            Lesson? lesson = await _context.Lessons.FirstOrDefaultAsync(l=> l.Id == validLessonId);

            if (lesson == null)
            {
                return RedirectToAction("Index", "Course");
            }

            UserLessonProgress userLessonProgress = new()
            {
                LessonId = validLessonId,
                UserId = GetUserId(),
                IsCompleted = true,
                CompletionDate = DateTime.Now,
            };

            await _context.UsersLessonsProgresses.AddAsync(userLessonProgress);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Course", new {id = validCourseId});
        }
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
    }
}
