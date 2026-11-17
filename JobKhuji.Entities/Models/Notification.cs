
using System;
using System.ComponentModel.DataAnnotations;

namespace JobKhuji.Entities.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        // Notification recipient
        [Required]
        public string UserId { get; set; } = string.Empty;

        // Notification content
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;

        // Read status
        public bool IsRead { get; set; } = false;

        // Creation date
        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        public DateTime? ReadAt { get; set; }

        // Relationship with ApplicationUser
        public ApplicationUser User { get; set; } = null!;
    }
}
