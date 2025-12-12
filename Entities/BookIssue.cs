// Entities/BookIssue.cs → FIXED

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practice_Project.Entities
{
    public class BookIssue
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }
        public int StudentId { get; set; }

        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Issued";

        [Column(TypeName = "decimal(10,2)")]
        public decimal FineAmount { get; set; } = 0;

        // Correct: Reference actual Entities
        public virtual Book Book { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
        public virtual Fine? Fine { get; set; }
    }
}