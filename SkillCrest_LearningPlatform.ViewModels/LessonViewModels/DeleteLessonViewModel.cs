using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.LessonViewModels
{
    public class DeleteLessonViewModel
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required Guid CreatorId { get; set; }
        public required string DateCreated { get; set; }
        public required Guid CourseId { get; set; }
    }
}
