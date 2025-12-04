using Microsoft.EntityFrameworkCore;
using Practice_Project.Models;

namespace Practice_Project.Data;

public class LibraryDbContext :DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options):base (options)
    {
       
    }

    public DbSet<Books> Books { get; set; }
    public DbSet<Authors> Authors { get; set; }
    public DbSet<BookIssue > BookIssues { get; set; }
    public DbSet<Categorys> Categories { get; set; }
    public DbSet<Fine> Fines { get; set; }
    public DbSet<Student> Students { get; set; }
    
}