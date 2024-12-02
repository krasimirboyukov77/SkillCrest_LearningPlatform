using System.ComponentModel.DataAnnotations;
using SkillCrest_LearningPlatform.Common.Lesson;
using SkillCrest_LearningPlatform.Common;

namespace SkillCrest_LearningPlatform.ViewModels.LessonViewModels
{
    public class CreateLessonViewModel
    {
        [Required(ErrorMessage = ErrorMessages.RequiredErrorMessage)]
        [StringLength(
            ValidationConstants.LessonTitleMaxLength,
            ErrorMessage =ErrorMessages.LessonTitleErrorMessage,
            MinimumLength = ValidationConstants.LessonTitleMinLength)]
        public string Title { get; set; } = null!;

        [MaxLength(
            ValidationConstants.LessonDescriptionMaxLength, 
            ErrorMessage =ErrorMessages.LessonDescriptionErrorMessage)]
        public string? Description { get; set; }
        public string? DueDate { get; set; } 

        [Required]
        public string CourseId { get; set; } = null!;

        [Range(0,int.MaxValue)]
        public int Points { get; set; }

        public string? FilePath { get; set; }
        public string? FileName { get; set; }
    }
}
