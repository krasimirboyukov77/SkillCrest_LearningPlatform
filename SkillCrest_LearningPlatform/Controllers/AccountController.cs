using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[Authorize]
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

    [AllowAnonymous]
    [HttpGet]
    public IActionResult RegisterTeacher()
    {
        return View();
    }
    [AllowAnonymous]
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
    [AllowAnonymous]
    [HttpGet]
    public IActionResult RegisterStudent()
    {
        return View();
    }
    [AllowAnonymous]
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
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = "")
    {
        return View(new LoginViewModel());
    }
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        var user = await _userManager.FindByNameAsync(loginViewModel.Email);
        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Course");
            }
        }
        ModelState.AddModelError("", "Invalid username or password");
        return View(loginViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Ensure CSRF protection
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync(); // This signs the user out
        return RedirectToAction("Index", "Home"); // Redirect to a suitable action
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var userId = GetUserId();

        if(userId != Guid.Empty)
        {
            var currentUser = await _userManager.FindByIdAsync(userId.ToString());

            if (currentUser != null)
            {
                ProfileViewModel profile = new()
                {
                    Email = currentUser.Email ?? string.Empty,
                    FullName = currentUser.FullName,
                    SchoolName = currentUser.SchoolName,
                    UserImage = currentUser.UserImage,
                    Biography = currentUser.Biography,
                    Interests = currentUser.Interests,
                };
                return View(profile);
            }
            else
            {
                return NotFound();
            }
        }
        else
        {
            return BadRequest();
        }
    }
    private async Task EnsureRoleExists(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = roleName });
        }
    }
    private Guid GetUserId()
    {
        if(Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userGuid))
        {
            return userGuid;
        }

        return Guid.Empty;
    }
}
