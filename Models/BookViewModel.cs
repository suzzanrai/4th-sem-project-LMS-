using Microsoft.AspNetCore.Mvc.Rendering;

namespace Practice_Project.Models;

public class BookViewModel
{
    public Books Books { get; set; }
    
    public IEnumerable<SelectListItem> Authors { get; set; }
    
    public IEnumerable<SelectListItem> Categories { get; set; }
}