
using System;
using System.ComponentModel.DataAnnotations;

namespace JobKhuji.Entities.Models
{
    public class EmployerProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? CompanyDescription { get; set; }

        [MaxLength(200)]
        public string? CompanyAddress { get; set; }

        [MaxLength(100)]
        public string? Industry { get; set; }

        [MaxLength(200)]
        public string? Website { get; set; }

        [MaxLength(100)]
        public string? ContactEmail { get; set; }

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Relationship with ApplicationUser
        public ApplicationUser User { get; set; } = null!;
    }
}
