using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.QuizViewModels
{
    public class CreateQuizViewModel
    {
        public string Title { get; set; } = null!;
        public List<CreateQuestionViewModel> Questions { get; set; } = new List<CreateQuestionViewModel>();
    }
}
