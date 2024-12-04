using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.StatisticsViewModels
{
    public class StatisticsSubmissionViewModel
    {
        public required string Id { get; set; }
        public required string FilePath { get; set; } = null!;
        public required string FileName { get; set; } = null!;
        public required string UploaderName { get; set; } = null!;
    }
}
