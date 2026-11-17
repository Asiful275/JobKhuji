
using System;
using System.ComponentModel.DataAnnotations;

namespace JobKhuji.Entities.Models
{
    public class SavedJob
    {
        [Key]
        public int Id { get; set; }

        // Foreign Keys
        [Required]
        public int JobSeekerProfileId { get; set; }

        [Required]
        public int JobId { get; set; }

        // Date when the job was saved
        public DateTime SavedAt { get; set; }
            = DateTime.UtcNow;

        // Navigation Properties
        public JobSeekerProfile JobSeekerProfile { get; set; }
            = null!;

        public Job Job { get; set; } = null!;
    }
}
