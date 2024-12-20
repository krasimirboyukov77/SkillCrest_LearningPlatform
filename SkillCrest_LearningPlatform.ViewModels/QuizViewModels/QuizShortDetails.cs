﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.ViewModels.QuizViewModels
{
    public class QuizShortDetails
    {
        public required Guid Id {  get; set; }
        public required string Title { get; set; }

        public int TotalScore { get; set; }
        public int Score { get; set; }
        public bool IsSubmitted { get; set; }

        public string? Submitter { get; set; }
    }
}
