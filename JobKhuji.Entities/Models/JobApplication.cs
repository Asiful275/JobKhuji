
using System;
using System.ComponentModel.DataAnnotations;
using JobKhuji.Entities.Enums;

namespace JobKhuji.Entities.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        // Foreign Keys
        [Required]
        public int JobId { get; set; }

        [Required]
        public int JobSeekerProfileId { get; set; }

        [Required]
        public int CvId { get; set; }

        // Application Information
        public DateTime AppliedAt { get; set; }
            = DateTime.UtcNow;

        public ApplicationStatus Status { get; set; }
            = ApplicationStatus.Applied;

        [MaxLength(2000)]
        public string? CoverLetter { get; set; }

        public DateTime? StatusUpdatedAt { get; set; }

        // Navigation Properties
        public Job Job { get; set; } = null!;

        public JobSeekerProfile JobSeekerProfile { get; set; }
            = null!;

        public Cv Cv { get; set; } = null!;
    }
}
