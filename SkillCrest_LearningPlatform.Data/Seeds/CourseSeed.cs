using Microsoft.AspNetCore.Identity;

using SkillCrest_LearningPlatform.Data.Data.Models;

namespace SkillCrest_LearningPlatform.Data.Seeds
{
    public class CourseSeed
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public CourseSeed(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this._userManager = userManager;
            this._context = context;
        }

        public async Task SeedAsync()
        {
            if (!_context.Courses.Any())
            {
                var courseList = await CourseList();

                foreach (var course in courseList)
                {
                    await _context.AddAsync(course);
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Course>> CourseList()
        {
            var teacherId = await _userManager.FindByNameAsync("teacher");

            List<Course> courses = new()
             {
                new Course()
                    {
                        Id = Guid.Parse("cb051a6b-c976-4697-b3c4-01e61831297b"),
                        Title = "Math",
                        Description = "Beginner math class.",
                        DateCreated = DateTime.Now,
                        CreatorId = teacherId.Id,
                        ImageUrl = string.Empty
                    },
                new Course()
                {
                        Id = Guid.Parse("6fc4da15-1ceb-436b-81ef-4280fc0da7c7"),
                        Title = "Art",
                        Description = "Art class for children.",
                        DateCreated = DateTime.Now,
                        CreatorId = teacherId.Id,
                        ImageUrl = string.Empty
                },
                 new Course()
                {
                        Id = Guid.Parse("32b4a499-e827-4ffa-8265-8fe066a7afe7"),
                        Title = "Strange Sports",
                        Description = "Learn about the crazy sports in the past.",
                        DateCreated = DateTime.Now,
                        CreatorId = teacherId.Id,
                        ImageUrl = string.Empty
                },
             };

            return courses;
        }
    }
}
