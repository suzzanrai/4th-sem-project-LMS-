// Entities/Fine.cs → Small improvement

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Practice_Project.Entities;

public class Fine
{
    [Key]
    public int FineId { get; set; }

    public int BookIssueId { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    public DateTime GeneratedDate { get; set; } = DateTime.Now;
    public DateTime? PaidDate { get; set; }
    public bool IsPaid { get; set; } = false;

    public virtual BookIssue BookIssue { get; set; } = null!;
}