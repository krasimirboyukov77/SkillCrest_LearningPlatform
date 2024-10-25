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
            return View();
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

            await _context.AddAsync(lesson);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new {id = lesson.Id});
        }
    }
}
