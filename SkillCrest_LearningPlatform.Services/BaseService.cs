using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.Data.Models;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;
using SkillCrest_LearningPlatform.Services.Interfaces;
using System.Security.Claims;
using SkillCrest_LearningPlatform.Common.Account;

namespace SkillCrest_LearningPlatform.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public BaseService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._userManager = userManager;
        }
        public bool IsGuidValid(string? id, ref Guid parsedGuid)
        {
           
            if (String.IsNullOrWhiteSpace(id))
            {
                return false;
            }
 
            bool isGuidValid = Guid.TryParse(id, out parsedGuid);
            if (!isGuidValid)
            {
                return false;
            }

            return true;
        }

        public Guid GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Guid.Empty;
            }
            if(Guid.TryParse(userId, out Guid userGuid))
            {

                return userGuid;
            }
            
            return Guid.Empty;
        }

        public async Task<bool> IsAdmin()
        {
            var user = await _userManager.FindByIdAsync(GetUserId().ToString());

            if (user == null)
            {
                return false;
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, ValidationConstants.TeacherRoleName);

            if (isAdmin == false)
            {
                return false;
            }

            return true;
        }
    }
}
