using Microsoft.AspNetCore.Mvc;
using SkillCrest_LearningPlatform.Models;
using System.Diagnostics;

namespace SkillCrest_LearningPlatform.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Home/Error")]
        public IActionResult Error(int? statusCode)
        {
            var model = new ErrorViewModel();

            if (statusCode == 404)
            {
                model.Message = "The page you are looking for could not be found.";
            }
            else if (statusCode == 500)
            {
                model.Message = "An error occurred while processing your request.";
            }
            else
            {
                model.Message = "An unexpected error occurred.";
            }

            model.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View(model);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
