using Microsoft.AspNetCore.Mvc;
using SkillCrest_LearningPlatform.Data.Models.QuizModels;
using SkillCrest_LearningPlatform.Data;
using SkillCrest_LearningPlatform.ViewModels.QuizViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Identity.Client;
using SkillCrest_LearningPlatform.Data.Models.Enum;

namespace SkillCrest_LearningPlatform.Controllers
{
    public class QuizController : Controller
    {
        private readonly ApplicationDbContext _context;  
   
        public QuizController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> Create(string courseId)
        {
            var model = new CreateQuizViewModel() { CourseId = Guid.Parse(courseId)};
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateQuizViewModel model)
        {
            if (ModelState.IsValid)
            {
                var quiz = new Quiz
                {
                    Title = model.Title,
                    DateCreated = DateTime.Now,
                    CreatorId = GetUserId(),
                    CourseId = model.CourseId
                };

                var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == model.CourseId);

                if (course != null)
                {
                    course.Quizzes.Add(quiz);
                }

                _context.Quizzes.Add(quiz); 
  
                foreach (var question in model.Questions)
                {
                    var newQuestion = new Question
                    {
                        QuizId = quiz.Id,
                        Text = question.Text,
                        Type = question.Type,
                    };
                    if(question.CorrectTextResponse == null)
                    {
                        newQuestion.CorrectTextResponse = "No answer.";
                    }
                    else
                    {
                        newQuestion.CorrectTextResponse = question.CorrectTextResponse;
                    }
                    _context.Questions.Add(newQuestion); 


                    foreach (var option in question.Options)
                    {
                        var newOption = new Option
                        {
                            QuestionId = newQuestion.Id,
                            Text = option.Text,
                            IsCorrect = option.IsCorrect
                        };

                        if (option.IsCorrect)
                        {
                            newQuestion.CorrectOptionId.Add(newOption.Id);
                        }

                        _context.Options.Add(newOption);
                    }
                }

                quiz.TotalScore = quiz.Questions.Count;

                _context.SaveChanges(); 


                return RedirectToAction("Index", "Course"); 
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Solve(string Id)
        {
            var quiz = await _context.Quizzes
                .Include(q=> q.Questions)
                .ThenInclude(q=> q.Options)
                .FirstOrDefaultAsync(q => q.Id.ToString() == Id);

            if(quiz == null)
            {
                return NotFound();
            }
            QuizSubmissionViewModel viewModel = new()
            {
                Id = quiz.Id,
                Title = quiz.Title,
                TotalScore = quiz.TotalScore,
                Questions = quiz.Questions.Select(q => new QuestionAnswerViewModel
                {
                    QuestionId = q.Id,
                    Text = q.Text,
                    Type = (int)q.Type,
                    CorrectTextResponse = "",
                    Options = q.Options.Select(option => new OptionViewModel
                    {
                        OptionId = option.Id,
                        Text = option.Text

                    }
                    ).ToList(),

                })
                .ToList(),
            };

            return View(viewModel); 
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitQuiz(QuizSubmissionViewModel viewModel)
        {
            int score = 0;
            if (ModelState.IsValid)
            {
                
                string userId = User.Identity.GetUserId();

                foreach (var questionAnswer in viewModel.Questions)
                {

                    var question = await _context.Questions.Include(q => q.Options)
                          .FirstOrDefaultAsync(q => q.Id == questionAnswer.QuestionId);

                    if (question == null) continue;

                    if (question.Type == QuestionType.OpenText)
                    {
                        if (question.CorrectTextResponse == "No anwser.")
                        {
                            score++;
                            questionAnswer.IsCorrect = true;
                        }
                        else if(questionAnswer.CorrectTextResponse == null)
                        {
                            questionAnswer.IsCorrect = false;
                        }
                       else if(question.CorrectTextResponse.ToLower().Trim() == questionAnswer.CorrectTextResponse.ToLower().Trim() 
                            && question.CorrectTextResponse != null 
                            && questionAnswer.CorrectTextResponse != null)
                        {
                            score++;
                            questionAnswer.IsCorrect = true;
                        }
                     
                        
                    }
                    else if (question.Type == QuestionType.MultipleChoice)
                    {

                        var correctOptions = question.Options.Where(o => o.IsCorrect == true).Select(o => o.Id).ToList();
                        var checkedOptions = questionAnswer.Options.Where(o=> o.SelectedOption == true).Select(o=> o.OptionId).ToList();

                        bool areEqual = correctOptions.OrderBy(o=> o).SequenceEqual(checkedOptions.OrderBy(o=> o));
                        if (areEqual)
                        {
                            questionAnswer.IsCorrect = true;
                            score++;
                        }
                       
                    }
                    else if(question.Type == QuestionType.RadioButton)
                    {
                        var questionSelectedAnswer = questionAnswer.Options.FirstOrDefault(o => o.SelectedOption == true);
                        if(questionSelectedAnswer != null)
                        {
                            var correctOption = question.CorrectOptionId.FirstOrDefault();
                            if(correctOption == questionSelectedAnswer.OptionId)
                            {
                                score++;
                                questionAnswer.IsCorrect = true;
                            }
                            else
                            {
                                questionAnswer.IsCorrect = false;
                            }
                        }
                    }
                }

                var quizSubmission = new QuizSubmission()
                {
                    QuizId = viewModel.Id,
                    StudentId = GetUserId(),
                    SubmissionDate = DateTime.Now,
                    Score = score,
                    TotalScore = viewModel.TotalScore,
                    Answers = viewModel.Questions.Select(q => new Answer()
                    {
                        QuestionId = q.QuestionId,
                        OptionsId = q.Options.Where(o => o.SelectedOption == true).Select(o => o.OptionId).ToList(),
                        TextResponse = q.CorrectTextResponse,
                        IsCorrect = q.IsCorrect

                    }).ToList()
                };

                _context.QuizSubmissions.Add(quizSubmission);   
                _context.SaveChanges();

                return RedirectToAction("Index", "Course");
            }

        
            return View(viewModel);
        }

        private Guid GetUserId()
        {
            if (Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userGuid))
            {
                return userGuid;
            }

            return Guid.Empty;
        }

        private bool IsCorrectOpenTextAnswer(string answer)
        {
  
            return !string.IsNullOrEmpty(answer);
        }
    }

   
}
