using Microsoft.AspNetCore.Mvc;
using SkillCrest_LearningPlatform.Data.Models.QuizModels;
using SkillCrest_LearningPlatform.Data;
using SkillCrest_LearningPlatform.ViewModels.QuizViewModels;

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
        public async Task<IActionResult> Create()
        {
            var model = new CreateQuizViewModel(); // Initialize an empty view model
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
                    DateCreated = DateTime.Now
                };

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

                        _context.Options.Add(newOption); // Add the option to the database context
                    }
                }

                // Step 4: Save the changes to the database
                _context.SaveChanges(); // Commit the transaction

                // Step 5: Redirect to a page (e.g., the list of quizzes or quiz details)
                return RedirectToAction("Index", "Course"); // Assuming there's a method to list quizzes
            }

            // If model is invalid, return to the form with the validation errors
            return View(model);
        }
    }
}
