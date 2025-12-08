// Entities/Category.cs
using System.ComponentModel.DataAnnotations;
using Practice_Project.Models;

namespace Practice_Project.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // One Category → Many Books
        public virtual ICollection<Books> Books { get; set; } = new List<Books>();
    }
}