using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.Data.Models;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;
using SkillCrest_LearningPlatform.Models;
using SkillCrest_LearningPlatform.Services.Interfaces;
using System.Diagnostics;

namespace SkillCrest_LearningPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBaseService _baseService;
        public HomeController(ILogger<HomeController> logger,
            IBaseService baseService)
        {
            _logger = logger;
            _baseService = baseService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
