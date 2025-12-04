using System.ComponentModel.DataAnnotations;

namespace Practice_Project.Models;

public class Categorys
{
    [Key]
    public int CategoryId { get; set; }

    [Required] [StringLength(50)] 
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    // Navigation
    public virtual ICollection<Books> Books { get; set; } = new List<Books>();
    

}