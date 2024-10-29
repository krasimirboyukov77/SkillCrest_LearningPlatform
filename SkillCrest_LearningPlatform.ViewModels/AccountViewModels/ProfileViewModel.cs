using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.AccountViewModels
{
    public class ProfileViewModel
    {
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required string SchoolName { get; set; }
        public string? UserImage { get; set; }
        public string? Interests { get; set; } 
        public string? Biography { get; set; }

    }
}
