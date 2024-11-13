using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCrest_LearningPlatform.Services;
using SkillCrest_LearningPlatform.Services.Interfaces;
using SkillCrest_LearningPlatform.ViewModels.CourseViewModels;


namespace SkillCrest_LearningPlatform.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseService _service;
        public CourseController(ICourseService service)
        {
            this._service = service;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _service.IndexGetCoursesByDateAsync();

            return View(courses);
        }
        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public IActionResult Create()
        {
            CreateCourseViewModel viewModel = new CreateCourseViewModel();

            return View(viewModel);
        }

        [Authorize(Roles ="Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            await _service.AddCourseAsync(viewModel);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            CourseDetailsViewModel? viewModel = await _service.GetDetailsAboutCourseAsync(id);

            if(viewModel == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var course = await _service.GetCourseForEditAsync(id);

            if(course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CourseEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var isUpdated = await _service.EditCourse(viewModel);

            if (isUpdated)
            {
                return RedirectToAction("Details", "Course", new { id = viewModel.Id });
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Users(string courseId)
        {
            var usersInCourse = await _service.GetUsersEnrolled(courseId);

            if (usersInCourse != null)
            {
                return View(usersInCourse);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Enroll(string courseId)
        {
            var courseGuid = await _service.EnrollStudent(courseId);

            if (courseGuid == false)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Details), new { id = courseId });
        }
    }
}
