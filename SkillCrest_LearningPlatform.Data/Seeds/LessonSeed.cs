using Microsoft.AspNetCore.Identity;

using SkillCrest_LearningPlatform.Data.Data.Models;


namespace SkillCrest_LearningPlatform.Data.Seeds
{
    public class LessonSeed
    {

        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;
        public LessonSeed(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this._userManager = userManager;
            this._context = context;
        }

        public async Task SeedAsync()
        {
            if (!_context.Lessons.Any())
            {
                var lessonlist = await LessonList();

                foreach (var lesson in lessonlist)
                {
                    await _context.AddAsync(lesson);
                }
                await _context.SaveChangesAsync();
            }

        }

        public async Task<List<Lesson>> LessonList()
        {
            var teacher = await _userManager.FindByNameAsync("teacher");

            List<Lesson> lessons = new()
             {
                //Math course lessons
                new Lesson()
                {
                    Title = "First steps in math.",
                    Description = "Today we will learn about the numbers.",
                    DateCreated = DateTime.Now,
                    CreatorId = teacher.Id,
                    CourseId = Guid.Parse("cb051a6b-c976-4697-b3c4-01e61831297b"),
                },
                new Lesson()
                {
                    Title = "Add and subtract",
                    Description = "In todays lesson we will learn how to add and subtract numbers.",
                    DateCreated = DateTime.Now,
                    CreatorId = teacher.Id,
                    CourseId = Guid.Parse("cb051a6b-c976-4697-b3c4-01e61831297b"),
                },
                 new Lesson()
                {
                    Title = "Multiply table",
                    Description = "This lesson is about the multiply table. We will learn how to multiply numbers and how to memorize the table faster. ",
                    DateCreated = DateTime.Now,
                    CreatorId = teacher.Id,
                    CourseId = Guid.Parse("cb051a6b-c976-4697-b3c4-01e61831297b"),
                },

                 //Art course lessons
                  new Lesson()
                {
                    Title = "Basic shadows",
                    Description = "This lesson is about shadows in paintings.",
                    DateCreated = DateTime.Now,
                    CreatorId = teacher.Id,
                    CourseId = Guid.Parse("6fc4da15-1ceb-436b-81ef-4280fc0da7c7"),
                },
                new Lesson()
                {
                    Title = "Color",
                    Description = "How to mix colors effectively and choose the righ one for your art.",
                    DateCreated = DateTime.Now,
                    CreatorId = teacher.Id,
                    CourseId = Guid.Parse("6fc4da15-1ceb-436b-81ef-4280fc0da7c7"),
                },
                 new Lesson()
                {
                    Title = "Landscape",
                    Description = "Upload a picture of your first landscape. ",
                    DateCreated = DateTime.Now,
                    CreatorId = teacher.Id,
                    CourseId = Guid.Parse("6fc4da15-1ceb-436b-81ef-4280fc0da7c7"),
                },

             };

            return lessons;
        }

    }
}
