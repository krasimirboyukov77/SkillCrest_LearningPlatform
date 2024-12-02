using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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

        public async Task<IActionResult> Index(string searchTerm)
        {

            var courses = await _service.IndexGetCoursesByDateAsync(searchTerm);

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
        public IActionResult EnrollPassword(string courseId)
        {
            var passwordViewModel = new CoursePasswordViewModel()
            {
                CourseId = courseId
            };

            return View(passwordViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(CoursePasswordViewModel viewModel)
        {
            var courseGuid = await _service.EnrollStudentWithPassword(viewModel);

            if (courseGuid == false)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Details), new { id = viewModel.CourseId });
        }


        [HttpGet]
        public async Task<IActionResult> EnrollNoPassword(string courseId)
        {
            var isSuccessful = await _service.EnrollStudentNoPassword(courseId);

            if (!isSuccessful)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Details), new { id = courseId });
        }

        [HttpGet]
        public async Task<IActionResult> Leave(string courseId)
        {
            var isSuccessful = await _service.LeaveCourse(courseId);

            if (!isSuccessful)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string courseId)
        {
            var isDeleted = await _service.DeleteCourse(courseId);

            if(isDeleted == false)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RemoveUser(string courseId, string userId)
        {
            var isSuccessful = await _service.RemoveUserFromCourse(courseId, userId);

            if(!isSuccessful)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
