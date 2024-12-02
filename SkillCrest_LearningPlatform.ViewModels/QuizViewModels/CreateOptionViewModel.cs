using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.QuizViewModels
{
    public class CreateOptionViewModel
    {
        public required string Text { get; set; } = null!;
        public required bool IsCorrect { get; set; }
    }
}
