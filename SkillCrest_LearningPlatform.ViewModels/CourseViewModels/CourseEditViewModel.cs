using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.CourseViewModels
{
    public class CourseEditViewModel
    {
        public required Guid Id { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(Common.Course.ValidationConstants.CourseTitleMaxLength, ErrorMessage = "Title must be between 5 and 128 characters!", MinimumLength = Common.Course.ValidationConstants.CourseTitleMinLength)]
        public required string Title { get; set; }
        public required string DateCreated { get; set; }

        [MaxLength(Common.Course.ValidationConstants.CourseDescriptionMaxLength, ErrorMessage = "Description must be maximum 500 characters!")]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
    }
}
