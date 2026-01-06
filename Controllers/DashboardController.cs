using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Practice_Project.Data;
using Practice_Project.Entities;
using Practice_Project.Models;

namespace Practice_Project.Controllers
{
    public class DashboardController : Controller
    {
        private readonly LibraryDbContext _context;

        public DashboardController(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel();

            // Total Counts
            model.TotalStudents = await _context.Students.CountAsync();
            model.TotalBooks = await _context.Books.CountAsync();
            model.TotalAuthors = await _context.Authors.CountAsync();
            model.TotalCategories = await _context.Categories.CountAsync();

            // Currently Issued Books
            var issuedCount = await _context.BookIssues
                .Where(bi => bi.ReturnDate == null)
                .CountAsync();

            model.CurrentlyIssued = issuedCount;
            model.AvailableBooks = model.TotalBooks - issuedCount;

            // Since overdue list is removed, we set these to 0
            model.OverdueCount = 0;
            model.TotalPendingFine = "$0.00";

            return View(model);
        }
    }
}