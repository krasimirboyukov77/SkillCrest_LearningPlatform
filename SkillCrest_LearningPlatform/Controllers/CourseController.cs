using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    }
}
