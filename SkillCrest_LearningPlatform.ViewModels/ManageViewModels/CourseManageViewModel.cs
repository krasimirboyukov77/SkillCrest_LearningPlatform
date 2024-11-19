using SkillCrest_LearningPlatform.ViewModels.LessonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.ManageViewModels
{
    public class CourseManageViewModel
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required string DateCreated { get; set; }
        public required string CreatorName { get; set; }

        public ICollection<LessonManageViewModel> Lessons { get; set; } = new HashSet<LessonManageViewModel>();
    }
}
