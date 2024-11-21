using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillCrest_LearningPlatform.Common;
using SkillCrest_LearningPlatform.Data.Models.QuizModels;

namespace SkillCrest_LearningPlatform.Data.Data.Models
{
    public class Course
    {
        public Course()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(Common.Course.ValidationConstants.CourseTitleMaxLength)]
        public string Title { get; set; } = null!;

        [MaxLength(Common.Course.ValidationConstants.CourseDescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public string? Password { get; set; }   
        public string? ImageUrl { get; set; } 

        [Required]
        public Guid CreatorId { get; set; }
        [Required]
        [ForeignKey(nameof(CreatorId))]
        public ApplicationUser Creator { get; set; } = null!;

        public ICollection<Lesson> Lessons { get; set; } = new HashSet<Lesson>();

        public ICollection<UserCourse> UsersCourses { get; set; } = new HashSet<UserCourse>();  

        public ICollection<Quiz>  Quizzes { get; set; } = new HashSet<Quiz>();
    }
}
