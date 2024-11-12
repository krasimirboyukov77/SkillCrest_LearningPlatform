using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using SkillCrest_LearningPlatform.Data.Models.QuizModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Data.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
        }

        public string FullName { get; set; } = null!;
        public string SchoolName { get; set; } = null!;
        public string? Biography { get; set; }
        public string? UserImage {  get; set; } 
        public string? Interests { get; set; }

        public ICollection<UserCourse> UsersCourses { get; set; } = new HashSet<UserCourse>();
        public ICollection<Lesson> Lessons { get; set; } = new HashSet<Lesson>();
        public ICollection<UserLessonProgress> UsersLessonsProgresses { get; set; } = new HashSet<UserLessonProgress>();
        public ICollection<QuizSubmission> QuizSubmissions { get; set;} = new HashSet<QuizSubmission>();
    }
}
