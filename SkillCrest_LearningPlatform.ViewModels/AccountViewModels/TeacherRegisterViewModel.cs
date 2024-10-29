using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillCrest_LearningPlatform.Common.Account;

namespace SkillCrest_LearningPlatform.ViewModels.AccountViewModels
{
    public class TeacherRegisterViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "This field is required")]
        [StringLength(ValidationConstants.PasswordMaxLength, ErrorMessage = "Must be between 6 and 128 characters!", MinimumLength = ValidationConstants.PasswordMinLength)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "This field is required")]
        [StringLength(ValidationConstants.FullNameMaxLength, ErrorMessage = "Must be between 4 and 264 characters!", MinimumLength = ValidationConstants.FullNameMinLength)]
        public string FullName { get; set; } = null!;
        [Required(ErrorMessage = "This field is required")]
        [StringLength(ValidationConstants.SchoolNameMaxLength, ErrorMessage = "Must be between 10 and 128 characters!", MinimumLength = ValidationConstants.SchoolNameMinLength)]
        public required string SchoolName { get; set; }

        [MaxLength(ValidationConstants.ImageUrlMaxLength)]
        public string? UserImage { get; set; }

        [MaxLength(ValidationConstants.InterestsMaxLength)]
        public string? Interests {  get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(ValidationConstants.BiographyMaxLength, ErrorMessage = "Must be between 10 and 128 characters!", MinimumLength = ValidationConstants.BiographyMinLength)]
        public string Biography { get; set; } = null!;
    }
}

