using SkillCrest_LearningPlatform.Data.Models.Enum;


namespace SkillCrest_LearningPlatform.ViewModels.QuizViewModels
{
    public class CreateQuestionViewModel
    {
        public string Text { get; set; } = null!;
        public QuestionType Type { get; set; } 
        public string? CorrectTextResponse { get; set; }
        public List<CreateOptionViewModel> Options { get; set; } = new List<CreateOptionViewModel>();
    }
}
