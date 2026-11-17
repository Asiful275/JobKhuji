
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobKhuji.Entities.Models
{
    public class MatchScore
    {
        [Key]
        public int Id { get; set; }

        // Foreign Keys
        public int JobId { get; set; }

        public int CvId { get; set; }

        // AI Matching Percentage (0 to 100)
        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 100)]
        public decimal Score { get; set; }

        // Matching Algorithm
        [Required]
        [MaxLength(50)]
        public string Algorithm { get; set; }
            = "TF-IDF + Cosine Similarity";

        // Last calculation time
        public DateTime CalculatedAt { get; set; }
            = DateTime.UtcNow;

        // Navigation Properties
        public Job Job { get; set; } = null!;

        public Cv Cv { get; set; } = null!;
    }
}
