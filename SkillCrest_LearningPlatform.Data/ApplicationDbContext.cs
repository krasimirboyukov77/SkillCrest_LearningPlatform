using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.Data.Models;
using SkillCrest_LearningPlatform.Data.Models.QuizModels;

namespace SkillCrest_LearningPlatform.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<UserCourse> UsersCourses { get; set; }
        public virtual DbSet<UserLessonProgress> UsersLessonsProgresses { get; set; }
        public virtual DbSet<Submission> Submissions { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }


        //Entities for Quiz
        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<QuizSubmission> QuizSubmissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserCourse>()
                .HasKey(uc => new {uc.CourseId, uc.UserId});
            builder.Entity<UserLessonProgress>()
                .HasKey(ulp => new {ulp.LessonId,ulp.UserId});

            builder.Entity<Course>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<Lesson>().HasQueryFilter(l => !l.IsDeleted);
            builder.Entity<Quiz>().HasQueryFilter(q => !q.IsDeleted);
        }
    }
}
