using SkillCrest_LearningPlatform.Data.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Data.Models
{
    public class Submission
    {
        public Submission()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        [Required]
        public string FilePath { get; set; } = null!;

        [Required]
        public string FileName { get; set; } = null!;

        [Required]
        public Guid LessonId { get; set; }

        [ForeignKey(nameof(LessonId))]
        public Lesson Lesson { get; set; } = null!;

        [Required]
        public Guid UploaderId { get; set; }
        [ForeignKey(nameof(UploaderId))]
        public ApplicationUser Uploader { get; set; } = null!;
    }
}
