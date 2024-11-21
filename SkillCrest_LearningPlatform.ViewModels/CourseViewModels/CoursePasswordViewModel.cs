using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.CourseViewModels
{
    public class CoursePasswordViewModel
    {
        public required string CourseId { get; set; }
        public string Password { get; set; } = null!;
    }
}
