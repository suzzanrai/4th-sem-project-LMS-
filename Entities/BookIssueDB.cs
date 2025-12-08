// Entities/BookIssue.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Practice_Project.Models;

namespace Practice_Project.Entities
{
    public class BookIssue
    {
        [Key]
        public int Id { get; set; }

        // Foreign keys
        public int BookId { get; set; }
        public int StudentId { get; set; }

        // Dates
        public DateTime IssueDate { get; set; } = DateTime.Now;

        [Column(TypeName = "date")] // optional: stores only date, no time
        public DateTime DueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReturnDate { get; set; }

        // Status: Issued, Returned, Overdue, Lost, etc.
        [StringLength(20)]
        public string Status { get; set; } = "Issued";

        // Fine
        [Column(TypeName = "decimal(10,2)")]
        public decimal FineAmount { get; set; } = 0;

        // Navigation properties (EF Core uses these for relationships)
        public virtual Books Book { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}