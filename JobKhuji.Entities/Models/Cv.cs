
using System;
using System.ComponentModel.DataAnnotations;

namespace JobKhuji.Entities.Models
{
    public class Cv
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int JobSeekerProfileId { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public string ExtractedText { get; set; }
            = string.Empty;

        public DateTime UploadedAt { get; set; }
            = DateTime.UtcNow;

        // Relationship with JobSeekerProfile
        public JobSeekerProfile JobSeekerProfile { get; set; }
            = null!;
    }
}
