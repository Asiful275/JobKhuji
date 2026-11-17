
using System;
using System.ComponentModel.DataAnnotations;

namespace JobKhuji.Entities.Models
{
    public class JobSeekerProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Phone { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        public string? Skills { get; set; }

        public string? Education { get; set; }

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        // Relationship with ApplicationUser
        public ApplicationUser User { get; set; } = null!;
    }
}
