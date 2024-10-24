using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Common.Course
{
    public class ValidationConstants
    {
        public const int CourseTitleMinLength = 5;
        public const int CourseTitleMaxLength = 128;

        public const int CourseDescriptionMaxLength = 500;

        public const string CourseDateCreatedFormat = "dd/MM/yyyy";
    }
}
