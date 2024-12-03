using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.StatisticsViewModels
{
    public class CourseStatisticsViewModel
    {
        public required string CourseName { get; set; } = null!;
        public required int TotalLessons { get; set; }
        public required double CompletedPercentage { get; set; }
        public required int CompletedLessons { get; set; }
    }
}
