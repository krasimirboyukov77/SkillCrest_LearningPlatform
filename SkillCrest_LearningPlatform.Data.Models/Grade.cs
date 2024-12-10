using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillCrest_LearningPlatform.Data.Models
{
    public class Grade
    {
        public Grade()
        {
            this.Id = Guid.NewGuid();   
        }
        public Guid Id { get; set; }

        [Required]
        public Guid SubmissionId { get; set; }

        [ForeignKey(nameof(SubmissionId))]
        public Submission Submission { get; set; } = null!;

        [Required]
        public double Score { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }


    }
}
