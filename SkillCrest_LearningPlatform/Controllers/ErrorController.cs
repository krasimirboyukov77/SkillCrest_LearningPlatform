using Microsoft.AspNetCore.Mvc;

namespace SkillCrest_LearningPlatform.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("404");
            }
            else if (statusCode == 500)
            {
                return View("500");
            }
            else
            {
                return View("Error");
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
