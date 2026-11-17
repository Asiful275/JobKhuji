
using System;
using System.ComponentModel.DataAnnotations;
using JobKhuji.Entities.Enums;

namespace JobKhuji.Entities.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Requirements { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        public decimal? MinimumSalary { get; set; }

        public decimal? MaximumSalary { get; set; }

        public JobType JobType { get; set; }

        public JobStatus Status { get; set; }
            = JobStatus.Pending;

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        public DateTime? Deadline { get; set; }

        // Foreign Keys
        [Required]
        public int EmployerProfileId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        // Navigation Properties
        public EmployerProfile EmployerProfile { get; set; }
            = null!;

        public Category Category { get; set; }
            = null!;
    }
}
