
using System;
using System.ComponentModel.DataAnnotations;

namespace JobKhuji.Entities.Models
{
    public class JobReport
    {
        [Key]
        public int Id { get; set; }

        // Reported Job
        [Required]
        public int JobId { get; set; }

        // User who submitted the report
        [Required]
        public string UserId { get; set; } = string.Empty;

        // Reason for reporting
        [Required]
        [MaxLength(1000)]
        public string Reason { get; set; } = string.Empty;

        // Report review status
        public bool IsReviewed { get; set; } = false;

        // Date of report submission
        public DateTime ReportedAt { get; set; }
            = DateTime.UtcNow;

        public DateTime? ReviewedAt { get; set; }

        // Navigation Properties
        public Job Job { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
    }
}
