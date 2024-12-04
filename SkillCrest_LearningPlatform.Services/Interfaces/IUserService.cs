using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> EnrollStudent(string courseId, string userId);
    }
}
