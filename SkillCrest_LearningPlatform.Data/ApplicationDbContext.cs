﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillCrest_LearningPlatform.Data.Data.Models;

namespace SkillCrest_LearningPlatform.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<UserCourse> UsersCourses { get; set; }
        public virtual DbSet<UserLessonProgress> UsersLessonsProgresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserCourse>()
                .HasKey(uc => new {uc.CourseId, uc.UserId});
            builder.Entity<UserLessonProgress>()
                .HasKey(ulp => new {ulp.LessonId,ulp.UserId});
        }
    }
}