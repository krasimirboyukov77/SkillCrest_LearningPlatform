using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillCrest_LearningPlatform.Common;


namespace SkillCrest_LearningPlatform.Data.Data.Models
{
    public class Lesson
    {
        public Lesson()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(Common.Lesson.ValidationConstants.LessonTitleMaxLength)]
        public string Title { get; set; } = null!;

        [MaxLength(Common.Lesson.ValidationConstants.LessonDescriptionMaxLength)]
        public string? Description { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public DateTime? DueDate {  get; set; }

        public int Points { get; set; }

        [Required]
        public bool IsDeleted { get; set; } 

        [Required]
        public Guid CreatorId { get; set; }
        [Required]
        [ForeignKey(nameof(CreatorId))]
        public ApplicationUser Creator { get; set; } = null!;

        public string? FilePath { get; set; }
        public string? FileName { get; set; }

        [Required]
        public Guid CourseId { get; set; }
        [Required]
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; } = null!;

        public ICollection<UserLessonProgress> UsersLessonsProgresses { get; set; } = new HashSet<UserLessonProgress>();
    }
}
