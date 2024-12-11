using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCrest_LearningPlatform.Data;
using SkillCrest_LearningPlatform.ViewModels.LessonViewModels;
using System.Globalization;
using SkillCrest_LearningPlatform.Common.Lesson;
using SkillCrest_LearningPlatform.Data.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using SkillCrest_LearningPlatform.Services.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;
using System.Net.NetworkInformation;
using System.Web;
using System.Runtime.CompilerServices;
using SkillCrest_LearningPlatform.Data.Models;

namespace SkillCrest_LearningPlatform.Controllers
{
    [Authorize]
    public class LessonController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILessonService _lessonService;
        public LessonController(ApplicationDbContext context, ILessonService lessonService)
        {
            _context = context;
            _lessonService = lessonService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var lesson = await _lessonService.GetLessonDetails(id);

            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet]
        public IActionResult Create(string courseId)
        {

            CreateLessonViewModel viewModel = new CreateLessonViewModel()
            {
                CourseId = courseId
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLessonViewModel viewModel, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            bool resultFromCreation = await _lessonService.CreateLesson(viewModel, file);

            if (resultFromCreation == false)
            {
                return View(viewModel);
            }

            return RedirectToAction("Details", "Course", new { id = viewModel.CourseId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLessonCompletion(string lessonId, string courseId)
        {
            bool isToggled = await _lessonService.ToggleLessonCompletion(lessonId, courseId);
            if (isToggled)
            {
                return RedirectToAction("Details", "Course", new { id = courseId });
            }

            return View("Index", "Course");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsIncomplete(string lessonId, string courseId)
        {


            var isToggled = await _lessonService.MarkAsIncomplete(lessonId, courseId);


            if (isToggled)
            {
                return RedirectToAction("Details", "Course", new { id = courseId });
            }

            return View("Index", "Course");
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string lessonId)
        {
            var lessonDetails = await _lessonService.GetLessonDetails(lessonId);

            if (null == lessonDetails)
            {
                return RedirectToAction("Index", "Course");
            }

            return View(lessonDetails);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LessonDetailsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var isUpdated = await _lessonService.EditLesson(viewModel);

            if (isUpdated)
            {
                return RedirectToAction("Details", "Lesson", new { id = viewModel.Id });
            }

            return View(viewModel);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(string lessonId)
        {
            var lessonToDelete = await _lessonService.GetLessonForDelete(lessonId);

            if (lessonToDelete == null)
            {
                return RedirectToAction("Index", "Course");
            }

            return View(lessonToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteLessonViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var isDeleteSuccessful = await _lessonService.DeleteLesson(viewModel);

            if (isDeleteSuccessful)
            {
                return RedirectToAction("Details", "Course", new { id = viewModel.CourseId });
            }

            return RedirectToAction("Index", "Course");
        }

        [HttpGet]
        public IActionResult Download(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound();
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);


            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }


            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = "application/octet-stream";

            return File(fileBytes, contentType, fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(IFormFile file, string lessonId)
        {
            var isUploaded = await _lessonService.UploadFile(file, lessonId);

            if (!isUploaded)
            {
                return BadRequest();
            }

            return RedirectToAction("Details","Lesson", new {id = lessonId});
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Evaluation(string submissionId, double grade)
        {
            if (grade < 2 || grade > 6)
            {
                return Json(new { success = false, message = "Invalid submission ID or grade." });
            }

            var isSuccessful = await _lessonService.EvaluationSubmission(submissionId, grade);

            if (isSuccessful)
            {
                return Json(new { success = true, message = "Grade submitted successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to submit grade." });
            }
        }
    }
}

