

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SkillCrest_LearningPlatform.Data;
using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.Data.Seeds;

namespace SkillCrest_LearningPlatform.Services.Extensions
{
    public static class Seed
    {

        public static IApplicationBuilder SeedCourses(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            UserManager<ApplicationUser>? userManager = serviceProvider
               .GetService<UserManager<ApplicationUser>>();

            var context = serviceProvider.GetService<ApplicationDbContext>();

            var seeder = new CourseSeed(userManager, context);

            Task.Run(async () =>
            {
                await seeder.SeedAsync();
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }

        public static IApplicationBuilder SeedLessons(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            UserManager<ApplicationUser>? userManager = serviceProvider
               .GetService<UserManager<ApplicationUser>>();

            var context = serviceProvider.GetService<ApplicationDbContext>();

            var seeder = new LessonSeed(userManager, context);

            Task.Run(async () =>
            {
                await seeder.SeedAsync();
            })
                .GetAwaiter()
                .GetResult();

            return app;
        }


    }
}
