using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Common
{
    public class ErrorMessages
    {
        public const string RequiredErrorMessage = "This field is required!";

        //Course Validation Messages
        public const string CourseDescriptionErrorMessage = "Description must be maximum 500 characters!";
        public const string CourseTitleErrorMessage = "Title must be between 5 and 128 characters!";

        //Lesson Validation Messages
        public const string LessonTitleErrorMessage = "Title must be between 4 and 128 characters!";
        public const string LessonDescriptionErrorMessage = "Must be maximum 700 characters!";

    }
}
