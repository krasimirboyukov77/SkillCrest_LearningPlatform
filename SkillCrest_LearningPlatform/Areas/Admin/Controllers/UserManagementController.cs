using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using SkillCrest_LearningPlatform.Common.Account;
using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;
using SkillCrest_LearningPlatform.Services.Interfaces;
using SkillCrest_LearningPlatform.ViewModels.AccountViewModels;


namespace SkillCrest_LearningPlatform.Areas.Admin.Controllers
{
    [Area(ValidationConstants.AdminRoleName)]
    [Authorize(Roles = ValidationConstants.AdminRoleName)]
    public class UserManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<UserCourse> _userCourseRepository;
        private readonly IUserService _userService;
        public UserManagementController(UserManager<ApplicationUser> userManager,
                                     RoleManager<IdentityRole<Guid>> roleManager,
                                     IRepository<Course> courseRepository,
                                     IRepository<UserCourse> userCourseRepository,
                                     IUserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _courseRepository = courseRepository;
            _userService = userService;
            _userCourseRepository = userCourseRepository;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var model = new List<UsersWithRolesViewModel>();
            var courses = await _courseRepository.GetAllAsync();


            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                UsersWithRolesViewModel newEntity = new()
                {
                    UserId = user.Id.ToString(),
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    AllRoles = _roleManager.Roles
                        .Select( role => new SelectListItem { Text = role.Name, Value = role.Name })
                        .ToList(),
                };

                var enrolledCourses = _userCourseRepository.GetAllAttached()
                    .Where(uc => uc.UserId == user.Id)
                    .Select(uc=> uc.CourseId).ToList();

                var coursesForEnlrolling = _courseRepository.GetAllAttached()
                    .Where(c => enrolledCourses.Contains(c.Id) == false)
                    .Select(c=> new SelectListItem() { Text = c.Title, Value = c.Id.ToString()})
                    .ToList();

                newEntity.AvailableCourses = coursesForEnlrolling;

                model.Add(newEntity);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (!await _roleManager.RoleExistsAsync(role)) return BadRequest("Invalid role.");

            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnrollInCourse(string userId, string courseId)
        {
            if (!await _userService.EnrollStudent(courseId, userId))
            {
                return BadRequest("Failed to enroll user in course.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
