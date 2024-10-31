using Microsoft.AspNetCore.Http;
using SkillCrest_LearningPlatform.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
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
    }
}
