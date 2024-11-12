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
    public class Quiz
    {

        public Quiz()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public int TotalScore { get; set; }

        [Required]
        public Guid CreatorId { get; set; }

        [Required]
        [ForeignKey(nameof(CreatorId))]
        public ApplicationUser Creator { get; set; } = null!;
        public List<Question> Questions { get; set; } = new List<Question>();
        public List<QuizSubmission> QuizSubmissions { get; set; } = new List<QuizSubmission>();
    }
}
