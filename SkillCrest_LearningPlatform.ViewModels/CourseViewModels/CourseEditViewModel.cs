using System.ComponentModel.DataAnnotations;
using SkillCrest_LearningPlatform.Common;

namespace SkillCrest_LearningPlatform.ViewModels.CourseViewModels
{
    public class CourseEditViewModel
    {
        public required Guid Id { get; set; }

        [Required(ErrorMessage = ErrorMessages.RequiredErrorMessage)]

        [StringLength(
            Common.Course.ValidationConstants.CourseTitleMaxLength, 
            ErrorMessage = ErrorMessages.CourseTitleErrorMessage, 
            MinimumLength = Common.Course.ValidationConstants.CourseTitleMinLength)]
        public required string Title { get; set; }
        public required string DateCreated { get; set; }

        [MaxLength(Common.Course.ValidationConstants.CourseDescriptionMaxLength,
            ErrorMessage = "Description must be maximum 500 characters!")]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
    }
}
