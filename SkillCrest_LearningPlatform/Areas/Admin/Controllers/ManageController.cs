using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCrest_LearningPlatform.Services.Interfaces;
using SkillCrest_LearningPlatform.Common.Account;

namespace SkillCrest_LearningPlatform.Areas.Admin.Controllers
{
    [Area(ValidationConstants.AdminRoleName)]
    [Authorize(Roles = ValidationConstants.AdminRoleName)]
    public class ManageController : Controller
    {
        private readonly IManageService _manageService;

        public ManageController(IManageService manageService)
        {
            _manageService = manageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var coursesToManage = await _manageService.GetCoursesForManage();

            return View(coursesToManage);
        }

        [HttpGet]
        public async Task<IActionResult> LessonsInCourse(string courseId)
        {
            var courseWithLessons = await _manageService.GetLessonsForCourseToManage(courseId);

            if (courseWithLessons == null)
            {
                return NotFound();
            }

            return View(courseWithLessons);
        }
    }
}
