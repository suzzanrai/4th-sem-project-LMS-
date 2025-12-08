// Entities/Fine.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_Project.Entities
{
    public class Fine
    {
        [Key]
        public int FineId { get; set; }                    // Primary key (better name than just Id)

        public int BookIssueId { get; set; }               // Foreign key

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public DateTime GeneratedDate { get; set; } = DateTime.Now;

        [Column(TypeName = "date")]
        public DateTime? PaidDate { get; set; }

        public bool IsPaid { get; set; } = false;

        // Navigation property
        public virtual BookIssue BookIssue { get; set; } = null!;
    }
}