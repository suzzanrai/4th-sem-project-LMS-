using System.ComponentModel.DataAnnotations;

namespace Practice_Project.Models
{
    public class Books
    {
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISBN is required.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ISBN must be exactly 13 characters.")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Publication Year is required.")]
        [Range(1900, 2100, ErrorMessage = "Publication Year must be between 1900 and 2100.")]
        public int PublicationYear { get; set; }

        [Required(ErrorMessage = "Total Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Total Quantity must be at least 1.")]
        public int TotalQuantity { get; set; }

        [Required(ErrorMessage = "Quantity Available is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity Available must be at least 0.")]
        public int QuantityAvailable { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PublicationDate { get; set; } = DateTime.Now;

        // Foreign Keys
        [Required(ErrorMessage = "Author is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Author.")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Category.")]
        public int CategoryId { get; set; }

        // Navigation Properties
        public virtual Authors? Author { get; set; } = null!;
        public virtual Categorys? Category { get; set; } = null!;
        public virtual ICollection<BookIssue>? BookIssues { get; set; } = new List<BookIssue>();
    }
}