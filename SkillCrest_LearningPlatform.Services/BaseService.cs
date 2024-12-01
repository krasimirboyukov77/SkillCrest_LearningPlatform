using Microsoft.AspNetCore.Http;
using SkillCrest_LearningPlatform.Data.Models;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;
using SkillCrest_LearningPlatform.Services.Interfaces;
using System.Security.Claims;

namespace SkillCrest_LearningPlatform.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Manager> _managerRepository;
        public BaseService(IHttpContextAccessor httpContextAccessor,
            IRepository<Manager> managerRepository)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._managerRepository = managerRepository;
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

        public bool IsManager()
        {
            var userId = GetUserId();

            var isManager = _managerRepository.GetByIdAsync(userId);

            if (isManager == null)
            {
                return false;
            }

            return true;
        }
    }
}
