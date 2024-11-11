using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.LessonViewModels
{
    public class LessonDetailsViewModel
    {
        public Guid? Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }

        public required string DueDate { get; set; }
        public required string DateCreated { get; set; }

        public int? Points { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }

        public required string Creator {  get; set; }
        
    }
}
