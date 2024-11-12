using SkillCrest_LearningPlatform.Data.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Data.Models.QuizModels
{
    public class QuizSubmission
    {
        public QuizSubmission()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        [Required]
        public Guid QuizId { get; set; }
        [ForeignKey(nameof(QuizId))]
        public Quiz Quiz { get; set; } = null!;

        [Required]
        public Guid StudentId { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        public int TotalScore { get; set; }

        [Required]
        [ForeignKey(nameof(StudentId))]
        public ApplicationUser Student { get; set; } = null!;
        [Required]
        public DateTime SubmissionDate { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}
