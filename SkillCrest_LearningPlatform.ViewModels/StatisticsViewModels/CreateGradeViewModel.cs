
using System.ComponentModel.DataAnnotations;

namespace SkillCrest_LearningPlatform.ViewModels.StatisticsViewModels
{
   public class CreateGradeViewModel
    {

        [MaxLength(500)]
        public string? Comment { get; set; }

        [Required]
        [Range(2,6)]
        public double Grade { get; set; }

        [Required]
        public string SubmissionId { get; set; } = null!;

    }
}
