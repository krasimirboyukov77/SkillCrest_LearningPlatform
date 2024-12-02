
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SkillCrest_LearningPlatform.Data.Data.Models;

using SkillCrest_LearningPlatform.Common.Account;

namespace SkillCrest_LearningPlatform.Infrastructure.ApplicationBuilderExtensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedAdministrator(this IApplicationBuilder app, string email, string username, string password)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            RoleManager<IdentityRole<Guid>>? roleManager = serviceProvider
                .GetService<RoleManager<IdentityRole<Guid>>>();
            IUserStore<ApplicationUser>? userStore = serviceProvider
                .GetService<IUserStore<ApplicationUser>>();
            UserManager<ApplicationUser>? userManager = serviceProvider
                .GetService<UserManager<ApplicationUser>>();

            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager),
                    $"Service for {typeof(RoleManager<IdentityRole<Guid>>)} cannot be obtained!");
            }

            if (userStore == null)
            {
                throw new ArgumentNullException(nameof(userStore),
                    $"Service for {typeof(IUserStore<ApplicationUser>)} cannot be obtained!");
            }

            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager),
                    $"Service for {typeof(UserManager<ApplicationUser>)} cannot be obtained!");
            }

            Task.Run(async () =>
            {
                bool roleAdminExists = await roleManager.RoleExistsAsync(ValidationConstants.AdminRoleName);
                bool roleTeacherExist = await roleManager.RoleExistsAsync(ValidationConstants.TeacherRoleName);
                IdentityRole<Guid>? adminRole = null;
                IdentityRole<Guid>? teacherRole = null;
                if (!roleAdminExists)
                {
                    adminRole = new IdentityRole<Guid>(ValidationConstants.AdminRoleName);

                    IdentityResult result = await roleManager.CreateAsync(adminRole);
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException($"Error occurred while creating the {ValidationConstants.AdminRoleName} role!");
                    }
                }
                else
                {
                    adminRole = await roleManager.FindByNameAsync(ValidationConstants.AdminRoleName);
                }

                if (!roleTeacherExist)
                {
                    teacherRole = new IdentityRole<Guid>(ValidationConstants.TeacherRoleName);

                    IdentityResult result = await roleManager.CreateAsync(teacherRole);
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException($"Error occurred while creating the {ValidationConstants.TeacherRoleName} role!");
                    }
                }
                else
                {
                    teacherRole = await roleManager.FindByNameAsync(ValidationConstants.TeacherRoleName);
                }

                ApplicationUser? adminUser = await userManager.FindByEmailAsync(email);
                if (adminUser == null)
                {
                    adminUser = await
                        CreateAdminUserAsync(email, username, password, userStore, userManager);
                }

                if (await userManager.IsInRoleAsync(adminUser, ValidationConstants.AdminRoleName) && await userManager.IsInRoleAsync(adminUser, ValidationConstants.TeacherRoleName))
                {
                    return app;
                }

                IdentityResult userResult = await userManager.AddToRoleAsync(adminUser, ValidationConstants.AdminRoleName);
                IdentityResult userResultForTeacher = await userManager.AddToRoleAsync(adminUser, ValidationConstants.TeacherRoleName);
                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException($"Error occurred while adding the user {username} to the {ValidationConstants.AdminRoleName} role!");
                }
                else if (!userResultForTeacher.Succeeded)
                {
                    throw new InvalidOperationException($"Error occurred while adding the user {username} to the {ValidationConstants.TeacherRoleName} role!");
                }

                return app;
            })
               .GetAwaiter()
               .GetResult();

            return app;
        }

        public static IApplicationBuilder SeedTeacher(this IApplicationBuilder app, string email, string username, string password)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateAsyncScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            RoleManager<IdentityRole<Guid>>? roleManager = serviceProvider
                .GetService<RoleManager<IdentityRole<Guid>>>();
            IUserStore<ApplicationUser>? userStore = serviceProvider
                .GetService<IUserStore<ApplicationUser>>();
            UserManager<ApplicationUser>? userManager = serviceProvider
                .GetService<UserManager<ApplicationUser>>();

            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager),
                    $"Service for {typeof(RoleManager<IdentityRole<Guid>>)} cannot be obtained!");
            }

            if (userStore == null)
            {
                throw new ArgumentNullException(nameof(userStore),
                    $"Service for {typeof(IUserStore<ApplicationUser>)} cannot be obtained!");
            }

            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager),
                    $"Service for {typeof(UserManager<ApplicationUser>)} cannot be obtained!");
            }

            Task.Run(async () =>
            {
                bool roleExists = await roleManager.RoleExistsAsync(ValidationConstants.TeacherRoleName);
                IdentityRole<Guid>? teacherRole = null;
                if (!roleExists)
                {
                    teacherRole = new IdentityRole<Guid>(ValidationConstants.TeacherRoleName);

                    IdentityResult result = await roleManager.CreateAsync(teacherRole);
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException($"Error occurred while creating the {ValidationConstants.TeacherRoleName} role!");
                    }
                }
                else
                {
                    teacherRole = await roleManager.FindByNameAsync(ValidationConstants.TeacherRoleName);
                }

                ApplicationUser? teacher = await userManager.FindByEmailAsync(email);
                if (teacher == null)
                {
                    teacher = await
                        CreateTeacherAsync(email, username, password, userStore, userManager);
                }

                if (await userManager.IsInRoleAsync(teacher, ValidationConstants.TeacherRoleName))
                {
                    return app;
                }

                IdentityResult userResult = await userManager.AddToRoleAsync(teacher, ValidationConstants.TeacherRoleName);
                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException($"Error occurred while adding the user {username} to the {ValidationConstants.TeacherRoleName} role!");
                }

                return app;
            })
               .GetAwaiter()
               .GetResult();

            return app;
        }

        private static async Task<ApplicationUser> CreateAdminUserAsync(string email, string username, string password,
            IUserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                Email = email,
                FullName = username,
                SchoolName = "no school"
            };

            await userStore.SetUserNameAsync(applicationUser, username, CancellationToken.None);
            IdentityResult result = await userManager.CreateAsync(applicationUser, password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error occurred while registering {ValidationConstants.AdminRoleName} user!");
            }

            return applicationUser;
        }

        private static async Task<ApplicationUser> CreateTeacherAsync(string email, string username, string password,
           IUserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                Email = email,
                FullName = username,
                SchoolName = "no school"
            };

            await userStore.SetUserNameAsync(applicationUser, username, CancellationToken.None);
            IdentityResult result = await userManager.CreateAsync(applicationUser, password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error occurred while registering {ValidationConstants.AdminRoleName} user!");
            }

            return applicationUser;
        }
    }
}