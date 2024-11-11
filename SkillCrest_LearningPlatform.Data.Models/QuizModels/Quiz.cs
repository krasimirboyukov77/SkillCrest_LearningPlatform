using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
