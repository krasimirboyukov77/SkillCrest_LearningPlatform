using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;
using SkillCrest_LearningPlatform.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Services
{
    public class UserService : BaseService,IUserService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<UserCourse> _userCourseRepository;

        public UserService(
            IRepository<Course> courseRepository,
            IRepository<UserCourse> userCourseRepository,
             IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
            : base(httpContextAccessor, userManager)
        {
            _courseRepository = courseRepository;
            _userCourseRepository = userCourseRepository;
        }
        public async Task<bool> EnrollStudent(string courseId, string userId)
        {
            Guid courseGuid = Guid.Empty;
            bool isValidCourseGuid = IsGuidValid(courseId, ref courseGuid);

            if (!isValidCourseGuid)
            {
                return false;
            }

            var course = await _courseRepository.FirstOrDefaultAsync(c => c.Id == courseGuid);

            if (course == null)
            {
                return false;
            }

            var user = await GetUser(userId);

            if (user == null)
            {
                return false;
            }

            var userCourse = await _userCourseRepository.FirstOrDefaultAsync(uc=> uc.CourseId == courseGuid && uc.UserId == user.Id);

            if (userCourse != null)
            {
                return false;
            }

            UserCourse newUserCourse = new UserCourse()
            {
                UserId = user.Id,
                CourseId = courseGuid,
            };

            await _userCourseRepository.AddAsync(newUserCourse);

            return true;
        }
    }
}
