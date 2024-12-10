using Microsoft.AspNetCore.Mvc;
using SkillCrest_LearningPlatform.Data.Models;
using SkillCrest_LearningPlatform.Services.Interfaces;
using SkillCrest_LearningPlatform.ViewModels.StatisticsViewModels;

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

            TempData["ReturnUrl"] = Url.Action("LessonSubmissions", "Statistics", new { lessonId });

            return View(submissions);
        }

        [HttpGet]
        public async Task<IActionResult> QuizSubmissions(string quizId)
        {
            var submissions = await _statisticsService.GetQuizSubmissions(quizId);

            return View(submissions);
        }

        [HttpGet]
        public IActionResult Evaluate(string submissionId)
        {
            var viewModel = new CreateGradeViewModel();

            viewModel.SubmissionId = submissionId;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Evaluate(CreateGradeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var isGraded = await _statisticsService.Evaluate(viewModel);

            if (!isGraded)
            {
                return BadRequest();
            }

            if (TempData["ReturnUrl"] is string returnUrl)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Course");
        }
    }
}
