
// Entities/Author.cs → FIXED

using System.ComponentModel.DataAnnotations;

namespace Practice_Project.Entities
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required][StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Biography { get; set; }

        // Correct: Reference Entity, not ViewModel
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}