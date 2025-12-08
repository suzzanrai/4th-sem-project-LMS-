// Entities/Author.cs
using System.ComponentModel.DataAnnotations;
using Practice_Project.Models;

namespace Practice_Project.Entities   // ← Important: Entities namespace
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Biography { get; set; }

        // Navigation property - one author has many books
        public virtual ICollection<Books> Books { get; set; } = new List<Books>();
    }
}