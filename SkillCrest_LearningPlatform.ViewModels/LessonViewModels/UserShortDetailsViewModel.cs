using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.LessonViewModels
{
    public class UserListViewModel
    {
        public required string CurrentCourseId { get; set; }
        public required string CreatorId { get; set; }

        public virtual List<UserShortDetailsViewModel> ShortDetails { get; set; } = new List<UserShortDetailsViewModel>();
    }

    public class UserShortDetailsViewModel
    {
        public required string UserName { get; set; }
        public required string Id { get; set; }
        public required string ImageUrl { get; set; } 
    }
}
