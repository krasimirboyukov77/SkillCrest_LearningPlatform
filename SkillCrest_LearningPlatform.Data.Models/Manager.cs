using SkillCrest_LearningPlatform.Data.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Data.Models
{
    public class Manager
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
    }
}
