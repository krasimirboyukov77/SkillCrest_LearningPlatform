using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillCrest_LearningPlatform.Data.Data.Models;
using System;
using System.Threading.Tasks;
using SkillCrest_LearningPlatform.ViewModels.AccountViewModels;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult RegisterTeacher()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterTeacher(TeacherRegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                UserImage = model.UserImage,
                Biography = model.Biography,
                Interests = model.Interests,
                SchoolName = model.SchoolName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await EnsureRoleExists("Teacher");
                await _userManager.AddToRoleAsync(user, "Teacher");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult RegisterStudent()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterStudent(StudentRegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                UserImage = model.UserImage,
                Interests = model.Interests,
                SchoolName = model.SchoolName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await EnsureRoleExists("Student");
                await _userManager.AddToRoleAsync(user, "Student");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Ensure CSRF protection
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync(); // This signs the user out
        return RedirectToAction("Index", "Home"); // Redirect to a suitable action
    }

    private async Task EnsureRoleExists(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = roleName });
        }
    }
}
