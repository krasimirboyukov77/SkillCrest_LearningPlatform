using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Data.Models.QuizModels
{
    public class Option
    {
        public Option()
        {
            this.Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        [Required]
        public string Text { get; set; } = null!;

        [Required]
        public bool IsCorrect { get; set; }

        [Required]
        public Guid QuestionId { get; set; }

        [Required]
        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; } = null!;
    }
}
