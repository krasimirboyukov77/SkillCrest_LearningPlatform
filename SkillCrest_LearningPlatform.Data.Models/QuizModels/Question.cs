using SkillCrest_LearningPlatform.Data.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Data.Models.QuizModels
{
    public class Question
    {
        public Question()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        [Required]
        public string Text { get; set; } = null!;

        [Required]
        public QuestionType Type { get; set; }

        [Required]
        public Guid QuizId { get; set; }

        [Required]
        [ForeignKey(nameof(QuizId))]
        public Quiz Quiz { get; set; } = null!;
        public List<Option> Options { get; set; } = new List<Option>();
        public List<Guid> CorrectOptionId { get; set; } = new List<Guid>();
        public string? CorrectTextResponse { get; set; }
    }
}
