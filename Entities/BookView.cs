using System.ComponentModel.DataAnnotations;
using Practice_Project.Models;

namespace Practice_Project.Entities;

public class BookView
{
    [Key]
    public int BookId { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(13)]
    public string ISBN { get; set; } = string.Empty;

    [Required]
    [Range(1450, 2100)]
    public int PublicationYear { get; set; }

    [Required]
    [Range(1, 10000)]
    public int TotalQuantity { get; set; }

    [Range(0, 10000)]
    public int QuantityAvailable { get; set; }

    [DataType(DataType.Date)]
    public DateTime? PublicationDate { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    // Foreign Keys
    [Required]
    public int AuthorId { get; set; }

    [Required]
    public int CategoryId { get; set; }

    // Navigation Properties
    public virtual Authors Author { get; set; } = null!;
    public virtual Categorys Category { get; set; } = null!;
    public virtual ICollection<BookIssue> BookIssues { get; set; } = new List<BookIssue>();
}