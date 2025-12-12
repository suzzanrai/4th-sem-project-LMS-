// Models/CreateAuthorViewModel.cs

using System.ComponentModel.DataAnnotations;

namespace Practice_Project.Models
{
    public class AuthorViewModel
    {
        public int AuthorId { get; set; }
        [Required][StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Biography { get; set; }
    }
}