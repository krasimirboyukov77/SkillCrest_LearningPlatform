using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Common.Account
{
    public class ValidationConstants
    {
        public const int FullNameMinLength = 4;
        public const int FullNameMaxLength = 264;

        public const int EmailMinLength = 10;
        public const int EmailMaxLength = 320;

        public const int PasswordMinLength = 6;
        public const int PasswordMaxLength = 128;

        public const int BiographyMinLength = 10;
        public const int BiographyMaxLength = 500;

        public const int ImageUrlMaxLength = 128;

        public const int InterestsMaxLength = 128;

        public const int SchoolNameMinLength = 10;
        public const int SchoolNameMaxLength = 128;

        public const string AdminRoleName = "Admin";
        public const string TeacherRoleName = "Teacher";
    }
}
