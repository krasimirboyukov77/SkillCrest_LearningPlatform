using Microsoft.AspNetCore.Mvc;
using SkillCrest_LearningPlatform.Services.Interfaces;

namespace SkillCrest_LearningPlatform.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public async Task<IActionResult> Index()
        {
            var statisticsForUser = await _statisticsService.GetCoureStatistic();

            return View(statisticsForUser);
        }

        [HttpGet]
        public async Task<IActionResult> LessonSubmissions(string lessonId)
        {
            var submissions = await _statisticsService.GetSubmissionsForLesson(lessonId);

            return View(submissions);
        }
    }
}
