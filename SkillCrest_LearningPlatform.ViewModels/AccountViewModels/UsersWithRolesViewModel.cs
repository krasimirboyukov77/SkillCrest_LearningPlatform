using Microsoft.AspNetCore.Mvc.Rendering;


namespace SkillCrest_LearningPlatform.ViewModels.AccountViewModels
{
    public class UsersWithRolesViewModel
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string> Roles { get; set; } = new List<string>();
        public List<SelectListItem> AllRoles { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableCourses { get; set; } = new List<SelectListItem>();
    }
}
