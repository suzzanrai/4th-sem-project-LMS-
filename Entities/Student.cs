// Entities/Student.cs
using System.ComponentModel.DataAnnotations;

namespace Practice_Project.Entities
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(30)]
        public string? RollNumber { get; set; }

        public bool IsActive { get; set; } = true;

        // One student → many book issues
   
        public virtual ICollection<BookIssue> BookIssues { get; set; } = new List<BookIssue>();
    }
}