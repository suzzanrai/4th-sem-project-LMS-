
    
/*
// Controllers/AuthorsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_Project.Data;
using Practice_Project.Entities;
using Practice_Project.Models;

namespace Practice_Project.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly LibraryDbContext _context;

        public AuthorsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Authors → List all authors (with book count)
        public async Task<IActionResult> Index()
        {
            var authors = await _context.Authors
                .Include(a => a.Books)           // Needed for @author.Books.Count in Index view
                .OrderBy(a => a.Name)
                .ToListAsync();

            return View(authors); // Returns List<Author> → matches your Index.cshtml
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View(new AuthorViewModel());
        }

        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var author = new Author
                {
                    Name = model.Name,
                    Biography = model.Biography
                };

                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            var model = new AuthorViewModel
            {
                AuthorId = author.AuthorId,
                Name = author.Name,
                Biography = author.Biography
            };

            return View(model);
        }

        // POST: Authors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuthorViewModel model)
        {
            if (id != model.AuthorId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var author = await _context.Authors.FindAsync(id);
                    if (author == null) return NotFound();

                    author.Name = model.Name;
                    author.Biography = model.Biography;

                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(id)) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Authors/Delete/5
        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var author = await _context.Authors
                .Include(a => a.Books)                 // ← THIS LINE IS MISSING!
                .FirstOrDefaultAsync(m => m.AuthorId == id);

            if (author == null) return NotFound();

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.AuthorId == id);
        }
    }
}*/


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_Project.Data;
using Practice_Project.Entities;
using Practice_Project.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_Project.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly LibraryDbContext _context;

        public AuthorsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Authors → List all authors (with book count)
        public async Task<IActionResult> Index()
        {
            // Manual SQL for PostgreSQL (quoted table & column names)
            var authors = await _context.Authors
                .FromSqlRaw("SELECT * FROM \"Authors\" ORDER BY \"Name\"")
                .ToListAsync();

            // Load books for each author manually to get book count
            foreach (var author in authors)
            {
                author.Books = await _context.Books
                    .FromSqlRaw("SELECT * FROM \"Books\" WHERE \"AuthorId\" = {0}", author.AuthorId)
                    .ToListAsync();
            }

            return View(authors);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View(new AuthorViewModel());
        }

        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Manual INSERT SQL
                await _context.Database.ExecuteSqlRawAsync(
                    "INSERT INTO \"Authors\" (\"Name\", \"Biography\") VALUES ({0}, {1})",
                    model.Name, model.Biography
                );

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Manual SQL to get author by ID
            var author = await _context.Authors
                .FromSqlRaw("SELECT * FROM \"Authors\" WHERE \"AuthorId\" = {0}", id)
                .FirstOrDefaultAsync();

            if (author == null) return NotFound();

            var model = new AuthorViewModel
            {
                AuthorId = author.AuthorId,
                Name = author.Name,
                Biography = author.Biography
            };

            return View(model);
        }

        // POST: Authors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuthorViewModel model)
        {
            if (id != model.AuthorId) return NotFound();

            if (ModelState.IsValid)
            {
                // Manual UPDATE SQL
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE \"Authors\" SET \"Name\" = {0}, \"Biography\" = {1} WHERE \"AuthorId\" = {2}",
                    model.Name, model.Biography, model.AuthorId
                );

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // Load author manually
            var author = await _context.Authors
                .FromSqlRaw("SELECT * FROM \"Authors\" WHERE \"AuthorId\" = {0}", id)
                .FirstOrDefaultAsync();

            if (author == null) return NotFound();

            // Load books for display (optional)
            author.Books = await _context.Books
                .FromSqlRaw("SELECT * FROM \"Books\" WHERE \"AuthorId\" = {0}", author.AuthorId)
                .ToListAsync();

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Manual DELETE SQL
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM \"Authors\" WHERE \"AuthorId\" = {0}", id
            );

            return RedirectToAction(nameof(Index));
        }

        // Optional: Check existence using manual SQL
        private async Task<bool> AuthorExists(int id)
        {
            var exists = await _context.Authors
                .FromSqlRaw("SELECT * FROM \"Authors\" WHERE \"AuthorId\" = {0}", id)
                .AnyAsync();

            return exists;
        }
    }
}
