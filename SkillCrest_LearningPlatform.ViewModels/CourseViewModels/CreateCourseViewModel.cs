using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillCrest_LearningPlatform.Common;

namespace SkillCrest_LearningPlatform.ViewModels.CourseViewModels
{
    public class CreateCourseViewModel
    {
        [Required(ErrorMessage ="This field is required!")]
        [StringLength(Common.Course.ValidationConstants.CourseTitleMaxLength, ErrorMessage ="Title must be between 5 and 128 characters!", MinimumLength =Common.Course.ValidationConstants.CourseTitleMinLength)]
        public string Title { get; set; } = null!;

        [MaxLength(Common.Course.ValidationConstants.CourseDescriptionMaxLength, ErrorMessage ="Description must be maximum 500 characters!")]
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
