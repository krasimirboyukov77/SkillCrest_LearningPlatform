using SkillCrest_LearningPlatform.ViewModels.LessonViewModels;
using SkillCrest_LearningPlatform.ViewModels.ManageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Services.Interfaces
{
    public interface IManageService
    {
        Task<IEnumerable<CourseManageViewModel>?> GetCoursesForManage();
        Task<CourseShortDetails?> GetLessonsForCourseToManage(string courseId);
    }
}
