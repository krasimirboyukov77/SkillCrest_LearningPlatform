using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.QuizViewModels
{
    public class QuizSubmissionViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public int TotalScore { get; set; }
        public List<QuestionAnswerViewModel> Questions { get; set; } = new List<QuestionAnswerViewModel>();
    }

    public class QuestionAnswerViewModel
    {
        public Guid QuestionId { get; set; }
        public string Text { get; set; } = null!;
        public int Type { get; set; }
        public string? CorrectTextResponse { get; set; }
        public bool IsCorrect { get; set; }
        public List<OptionViewModel> Options { get; set; } = new List<OptionViewModel>();
    }

    public class OptionViewModel
    {
        public Guid OptionId { get; set; }
        public string Text { get; set; } = null!;
        public bool SelectedOption { get; set; }
    }
}
