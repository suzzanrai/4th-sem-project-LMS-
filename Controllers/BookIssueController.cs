

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_Project.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Practice_Project.Data;
using Practice_Project.Entities;

namespace Practice_Project.Controllers
{
    public class BookIssueController : Controller
    {
        // 1. DbContext is injected - this gives access to the database
        private readonly LibraryDbContext _context;   // Replace ApplicationDbContext with your actual DbContext name

        // 2. Constructor - Dependency Injection provides the DbContext
        public BookIssueController(LibraryDbContext context)
        {
            _context = context;
        }

        // 3. GET: /BookIssue/ - Shows list of all issued books
        public async Task<IActionResult> Index()
        {
            // Load BookIssue records along with related Book and Student data
            var issues = await _context.BookIssues
                .Include(bi => bi.Book)      // Load Book details
                .Include(bi => bi.Student)   // Load Student details
                .ToListAsync();

            return View(issues);   // Pass the list to Index.cshtml view
        }

        // 4. GET: /BookIssue/Create - Shows form to issue a new book
        public IActionResult Create()
        {
            // Load books and students for dropdowns in the view
            ViewBag.Books = _context.Books.ToList();          // Assuming you have DbSet<BookViewModel> Books
            ViewBag.Students = _context.Students.ToList();    // Assuming you have DbSet<StudentVm> Students

            return View();
        }

        // 5. POST: /BookIssue/Create - Handles form submission to issue a book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookIssue issue)
        {
            if (ModelState.IsValid)
            {
                // Set default values if not provided
                issue.IssueDate = DateTime.Now;
                issue.Status = "Issued";
                issue.FineAmount = 0;

                // Calculate DueDate - here assuming 14 days
                issue.DueDate = DateTime.Now.AddDays(14);

                _context.BookIssues.Add(issue);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));  // Redirect to list after success
            }

            // If validation fails, reload dropdowns and show form again
            ViewBag.Books = _context.Books.ToList();
            ViewBag.Students = _context.Students.ToList();
            return View(issue);
        }

        // 6. GET: /BookIssue/Return/5 - Shows return page for a specific issue
        public async Task<IActionResult> Return(int id)
        {
            var issue = await _context.BookIssues
                .Include(bi => bi.Book)
                .Include(bi => bi.Student)
                .FirstOrDefaultAsync(bi => bi.Id == id);

            if (issue == null)
                return NotFound();

            return View(issue);
        }

        // 7. POST: /BookIssue/Return/5 - Processes the return of a book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id, BookIssue model)
        {
            var issue = await _context.BookIssues.FindAsync(id);
            if (issue == null)
                return NotFound();

            // Mark as returned
            issue.ReturnDate = DateTime.Now;
            issue.Status = "Returned";

            // Simple fine calculation: $1 per day after DueDate
            if (issue.ReturnDate > issue.DueDate)
            {
                int overdueDays = (issue.ReturnDate.Value - issue.DueDate).Days;
                issue.FineAmount = overdueDays * 1.00m;  // 1 dollar per day
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}