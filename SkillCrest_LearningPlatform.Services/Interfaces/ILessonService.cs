using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using SkillCrest_LearningPlatform.ViewModels.LessonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Services.Interfaces
{
    public interface ILessonService
    {
        Task<LessonDetailsViewModel?> GetLessonDetails(string id);
        Task<bool> UploadFile(IFormFile file, string courseId);
        Task<bool> CreateLesson(CreateLessonViewModel viewModel, IFormFile file);
        Task<bool> ToggleLessonCompletion(string lessonId, string courseId);
        Task<bool> MarkAsIncomplete(string lessonId, string courseId);
        Task<LessonDetailsViewModel?> GetLessonById(string lessonId);
        Task<bool> EditLesson(LessonDetailsViewModel viewModel);
        Task<DeleteLessonViewModel?> GetLessonForDelete(string lessonId);
        Task<bool> DeleteLesson(DeleteLessonViewModel viewModel);
    }
}
