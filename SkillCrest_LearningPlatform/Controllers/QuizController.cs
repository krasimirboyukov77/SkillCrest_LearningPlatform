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
        private readonly ApplicationDbContext _context;  // Assuming you are using Entity Framework for your database

        // Injecting the database context
        public QuizController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        // GET: Display the quiz creation form
        public async Task<IActionResult> Create(string courseId)
        {
            var model = new CreateQuizViewModel() { CourseId = Guid.Parse(courseId)};// Initialize an empty view model
            return View(model);
        }

        // POST: Handling the quiz form submission
        [HttpPost]
        public async Task<IActionResult> Create(CreateQuizViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Step 1: Create the quiz
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

                _context.Quizzes.Add(quiz); // Add the quiz to the database context

                // Step 2: Create the questions for this quiz
                foreach (var question in model.Questions)
                {
                    var newQuestion = new Question
                    {
                        QuizId = quiz.Id,
                        Text = question.Text,
                        Type = question.Type,
                        CorrectTextResponse = question.CorrectTextResponse
                    };
                    _context.Questions.Add(newQuestion); // Add the question to the database context

                    // Step 3: Add options to the question
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

                        _context.Options.Add(newOption); // Add the option to the database context
                    }
                }

                quiz.TotalScore = quiz.Questions.Count;
                // Step 4: Save the changes to the database
                _context.SaveChanges(); // Commit the transaction

                // Step 5: Redirect to a page (e.g., the list of quizzes or quiz details)
                return RedirectToAction("Index", "Course"); // Assuming there's a method to list quizzes
            }

            // If model is invalid, return to the form with the validation errors
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Solve(string Id)
        {
            var quiz = await _context.Quizzes
                .Include(q=> q.Questions)
                .ThenInclude(q=> q.Options)
                .FirstOrDefaultAsync(q => q.Id.ToString() == Id);

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
        public async Task<IActionResult> SubmitQuiz(QuizSubmissionViewModel viewModel)
        {
            int score = 0;
            if (ModelState.IsValid)
            {
                
                string userId = User.Identity.GetUserId();
                
                foreach (var questionAnswer in viewModel.Questions)
                {
                    // Retrieve the question from the database
                    var question = _context.Questions.Include(q=> q.Options)
                          .FirstOrDefault(q => q.Id == questionAnswer.QuestionId);

                    if (question == null) continue;


                    // Check the answer based on the question type
                    if (question.Type == QuestionType.OpenText)
                    {
                       if(question.CorrectTextResponse.ToLower().Trim() == questionAnswer.CorrectTextResponse.ToLower().Trim() 
                            && question.CorrectTextResponse != null 
                            && questionAnswer.CorrectTextResponse != null)
                        {
                            score++;
                            questionAnswer.IsCorrect = true;
                        }
                        
                    }
                    else if (question.Type == QuestionType.MultipleChoice)
                    {
                        // For multiple choice questions, check the selected option
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

                // Save the quiz result to the database
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

                // Redirect to the result page with the score
                return RedirectToAction("Index", "Course");
            }

            // If the model is invalid, return to the quiz page with validation errors
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
            // Add your custom logic here to check if the answer is correct
            // For now, assume any answer is correct
            return !string.IsNullOrEmpty(answer);
        }
    }

   
}
