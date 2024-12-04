using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SkillCrest_LearningPlatform.Common.Account;

namespace SkillCrest_LearningPlatform.Areas.Admin.Controllers
{
    [Area(ValidationConstants.AdminRoleName)]
    [Authorize(Roles = ValidationConstants.AdminRoleName)]
    public class UserManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
