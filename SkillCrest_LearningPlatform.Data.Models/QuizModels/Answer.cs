using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Data.Models.QuizModels
{
    public class Answer
    {
        public Answer()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        [Required]
        public Guid QuestionId { get; set; }
        [Required]
        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; } = null!;

        [Required]
        public Guid QuizSubmissionId { get; set; }

        [Required]
        [ForeignKey(nameof(QuizSubmissionId))]
        public QuizSubmission QuizSubmission { get; set; } = null!;
        public List<Guid> OptionsId { get; set; } = new List<Guid>();
        public string? TextResponse { get; set; }
        [Required]
        public bool IsCorrect { get; set; }
    }
}
