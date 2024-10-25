using Microsoft.AspNetCore.Mvc;

namespace SkillCrest_LearningPlatform.Controllers
{
    public class LessonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
