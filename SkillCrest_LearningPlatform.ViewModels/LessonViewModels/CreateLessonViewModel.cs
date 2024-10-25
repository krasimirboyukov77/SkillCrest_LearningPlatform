using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillCrest_LearningPlatform.Common.Lesson;

namespace SkillCrest_LearningPlatform.ViewModels.LessonViewModels
{
    public class CreateLessonViewModel
    {
        [Required(ErrorMessage = "This field is required!")]
        [StringLength(Common.Lesson.ValidationConstants.LessonTitleMaxLength, ErrorMessage ="Title must be between 5 and 128 characters!", MinimumLength =Common.Lesson.ValidationConstants.LessonTitleMinLength)]
        public string Title { get; set; } = null!;

        [MaxLength(Common.Lesson.ValidationConstants.LessonDescriptionMaxLength, ErrorMessage ="Must be maximum 700 characters!")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        public string DueDate { get; set; } = null!;

        [Required]
        public string CourseId { get; set; } = null!;
    }
}
