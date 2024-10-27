using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillCrest_LearningPlatform.Data;
using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.ViewModels.CourseViewModels;
using SkillCrest_LearningPlatform.ViewModels.LessonViewModels;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace SkillCrest_LearningPlatform.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private ApplicationDbContext _context;
        public CourseController(ApplicationDbContext context)
        {
                _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courseDetails = await _context.Courses
                .Include(c=> c.Creator)
                .Select(c=> new CourseInfoViewModel()
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description ?? string.Empty,
                DateCreated = c.DateCreated.ToString("dd/MM/yyyy"),
                Creator = c.Creator.UserName ?? string.Empty
                
                
            }).AsNoTracking()
            .ToListAsync();

            return View(courseDetails);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateCourseViewModel viewModel = new CreateCourseViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            Course course = new()
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                DateCreated = DateTime.Now,
                CreatorId = GetUserId(),
                ImageUrl = viewModel.ImageUrl
            };

            UserCourse userCourse = new()
            {
                UserId = GetUserId(),
                CourseId = course.Id,
            };

            await _context.Courses.AddAsync(course);
            await _context.UsersCourses.AddAsync(userCourse);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = course.Id});
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            bool isValidGuid = Guid.TryParse(id, out Guid courseGuid);

            if (!isValidGuid)
            {
                return RedirectToAction(nameof(Index));
            }

            var course = await _context.Courses
                .Include(c=> c.Lessons)
                .ThenInclude(c=> c.UsersLessonsProgresses)
                .Include(c=> c.Creator).FirstOrDefaultAsync(c => c.Id == courseGuid);

            if (course == null)
            {
                return RedirectToAction(nameof(Index));
            }

            CourseDetailsViewModel viewModel = new()
            {
                Id = course.Id,
                Title = course.Title,
                DateCreated = course.DateCreated.ToString("dd-MM-yyyy"),
                CreatorId = course.CreatorId,
                Lessons = course.Lessons.Select(l=> new LessonDetailsViewModel()
                {
                    Id = l.Id,
                    Title = l.Title,
                    Description = l.Description,
                    DateCreated= l.DateCreated.ToString("dd-MM-yyyy"),
                    DueDate = l.DueDate.ToString("dd-MM-yyyy"),
                    Creator = l.Creator.UserName ?? string.Empty,
                    IsCompleted = l.UsersLessonsProgresses.Any(ul=> ul.LessonId == l.Id && ul.UserId == GetUserId())
                }).ToList(),
            };

            return View(viewModel);
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
    }
}
