using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Common.Lesson
{
    public class ValidationConstants
    {
        public const int LessonTitleMinLength = 4;
        public const int LessonTitleMaxLength = 128;

        public const int LessonDescriptionMaxLength = 700;

        public const string LessonDateCreatedFormat = "dd/MM/yyyy";
    }
}
