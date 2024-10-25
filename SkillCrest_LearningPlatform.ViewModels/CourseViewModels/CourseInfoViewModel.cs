using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.CourseViewModels
{
    public class CourseInfoViewModel
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string DateCreated { get; set; }
        public required string Creator {  get; set; }
        
    }
}
