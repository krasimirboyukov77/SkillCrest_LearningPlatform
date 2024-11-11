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

            if(lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
       public IActionResult Create(string courseId)
        {

            CreateLessonViewModel viewModel = new CreateLessonViewModel()
            {
                CourseId = courseId
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateLessonViewModel viewModel, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            bool resultFromCreation = await _lessonService.CreateLesson(viewModel, file: null);

            if (resultFromCreation == false)
            {
                return View(viewModel);
            }

            return RedirectToAction("Details","Course", new {id = viewModel.CourseId});
        }

        [HttpPost]
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
        public async Task<IActionResult> MarkAsIncomplete(string lessonId , string courseId)
        {
           var isToggled = await _lessonService.MarkAsIncomplete(lessonId, courseId);


            if (isToggled)
            {
                return RedirectToAction("Details", "Course", new { id = courseId });
            }

            return View("Index", "Course");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string lessonId)
        {
            var lessonDetails = await _lessonService.GetLessonDetails(lessonId);

            if(null == lessonDetails)
            {
                return RedirectToAction("Index", "Course");
            }

            return View(lessonDetails);
        }

        [HttpPost]
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

            if(lessonToDelete == null)
            {
                return RedirectToAction("Index", "Course");
            }

            return View(lessonToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteLessonViewModel viewModel)
        {
            var isDeleteSuccessful = await _lessonService.DeleteLesson(viewModel);

            if (isDeleteSuccessful)
            {
                return RedirectToAction("Details", "Course", new {id = viewModel.CourseId});
            }

            return RedirectToAction("Index", "Course");
        }

        [HttpGet]
        public IActionResult Download(string fileName)
        {
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

            
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(); 
            }

           
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = "application/octet-stream";

            return File(fileBytes, contentType, fileName);
        }

    }
}
