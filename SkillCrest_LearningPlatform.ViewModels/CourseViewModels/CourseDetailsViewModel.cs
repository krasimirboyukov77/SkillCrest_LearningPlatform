﻿using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.ViewModels.LessonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.CourseViewModels
{
    public class CourseDetailsViewModel
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public required string DateCreated { get; set; }
        public required string CreatorId { get; set; }
        public string? Description { get; set; }

        public ICollection<LessonInCourseViewModel> Lessons { get; set; } = new HashSet<LessonInCourseViewModel>();   
    }
}
