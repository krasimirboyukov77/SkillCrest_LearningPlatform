using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.QuizViewModels
{
    public class QuizResult
    {
        public string UserId { get; set; } = null!; 
        public int Score { get; set; }
        public DateTime DateTaken { get; set; }
    }
}
