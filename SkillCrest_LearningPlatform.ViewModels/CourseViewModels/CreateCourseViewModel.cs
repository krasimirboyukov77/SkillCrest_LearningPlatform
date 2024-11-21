using System.ComponentModel.DataAnnotations;
using SkillCrest_LearningPlatform.Common;
using SkillCrest_LearningPlatform.Common.Course;

namespace SkillCrest_LearningPlatform.ViewModels.CourseViewModels
{
    public class CreateCourseViewModel
    {
        [Required(ErrorMessage = ErrorMessages.RequiredErrorMessage)]
        [StringLength(
            ValidationConstants.CourseTitleMaxLength,
            ErrorMessage =ErrorMessages.CourseTitleErrorMessage, 
            MinimumLength =ValidationConstants.CourseTitleMinLength)]
        public string Title { get; set; } = null!;

        [MaxLength(
            ValidationConstants.CourseDescriptionMaxLength,
            ErrorMessage =ErrorMessages.CourseDescriptionErrorMessage)]
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Password { get; set; }
    }
}
