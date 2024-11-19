using SkillCrest_LearningPlatform.ViewModels.LessonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.ManageViewModels
{

    public class CourseShortDetails
    {
        public required Guid CourseId { get; set; }
        public required string CourseTitle { get; set; }
        public ICollection<LessonManageViewModel> Lessons { get; set; } = new HashSet<LessonManageViewModel>();
    }
    public class LessonManageViewModel
    {
        public Guid? Id { get; set; }
        public required string Title { get; set; }
        public required string DueDate { get; set; }
        public required string DateCreated { get; set; }

    }
}
