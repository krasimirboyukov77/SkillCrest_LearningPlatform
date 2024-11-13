using SkillCrest_LearningPlatform.ViewModels.CourseViewModels;
using SkillCrest_LearningPlatform.ViewModels.LessonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Services.Interfaces
{
    public interface ICourseService 
    {
         Task<IEnumerable<CourseInfoViewModel>> IndexGetCoursesByDateAsync();

         Task AddCourseAsync(CreateCourseViewModel viewModel);

         Task<CourseDetailsViewModel?> GetDetailsAboutCourseAsync(string id);

        Task<CourseEditViewModel?> GetCourseForEditAsync(string id);
        Task<bool> EditCourse(CourseEditViewModel viewModel);

        Task<IEnumerable<UserShortDetailsViewModel>?> GetUsersEnrolled(string courseId);

        Task<bool> EnrollStudent(string courseId);
    }
}
