using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practice_Project.Data;
using Practice_Project.Models;

namespace Practice_Project.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category);
            return View(await books.ToListAsync());
        }

  
// GET: Books/Create
        public IActionResult Create()
        {
            // Fixed: use correct primary key property names → "AuthorId" and "CategoryId"
            ViewData["Authors"]    = new SelectList(_context.Authors.ToList(),    "AuthorId",   "Name");
            ViewData["Categories"] = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");

            return View();
        }

// POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Books book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns when validation fails
            ViewData["Authors"]    = new SelectList(_context.Authors.ToList(),    "AuthorId",   "Name", book.AuthorId);
            ViewData["Categories"] = new SelectList(_context.Categories.ToList(), "CategoryId", "Name", book.CategoryId);

            return View(book);
        }
        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            ViewData["Authors"] = new SelectList(_context.Authors, "AuthorId", "Name", book.AuthorId);
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "Name", book.CategoryId);
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Books book)
        {
            if (id != book.BookId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Authors"] = new SelectList(_context.Authors, "AuthorId", "Name", book.AuthorId);
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "Name", book.CategoryId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null) return NotFound();

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(b => b.BookId == id);
        }
    }
}
