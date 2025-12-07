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
            // Add a default "Select" option to dropdowns
            var authorItems = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Select Author --" } };
            authorItems.AddRange(_context.Authors.Select(a => new SelectListItem { Value = a.AuthorId.ToString(), Text = a.Name }));

            var categoryItems = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Select Category --" } };
            categoryItems.AddRange(_context.Categories.Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name }));

            ViewData["Authors"] = authorItems;
            ViewData["Categories"] = categoryItems;

            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Books book)
        {
            // Optional: Add custom validation (e.g., QuantityAvailable <= TotalQuantity)
            if (book.QuantityAvailable > book.TotalQuantity)
            {
                ModelState.AddModelError("QuantityAvailable", "Quantity Available cannot exceed Total Quantity.");
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the exception (in a real app, use logging framework)
                    ModelState.AddModelError("", $"An error occurred while saving: {ex.Message}");
                }
            }

            // Repopulate dropdowns when validation fails, with selected values
            var authorItems = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Select Author --" } };
            authorItems.AddRange(_context.Authors.Select(a => new SelectListItem { Value = a.AuthorId.ToString(), Text = a.Name }));

            var categoryItems = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Select Category --" } };
            categoryItems.AddRange(_context.Categories.Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name }));

            ViewData["Authors"] = new SelectList(authorItems, "Value", "Text", book.AuthorId.ToString());
            ViewData["Categories"] = new SelectList(categoryItems, "Value", "Text", book.CategoryId.ToString());

            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            // Add default "Select" for Edit as well, though usually not needed
            var authorItems = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Select Author --" } };
            authorItems.AddRange(_context.Authors.Select(a => new SelectListItem { Value = a.AuthorId.ToString(), Text = a.Name }));

            var categoryItems = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Select Category --" } };
            categoryItems.AddRange(_context.Categories.Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name }));

            ViewData["Authors"] = new SelectList(authorItems, "Value", "Text", book.AuthorId.ToString());
            ViewData["Categories"] = new SelectList(categoryItems, "Value", "Text", book.CategoryId.ToString());

            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Books book)
        {
            if (id != book.BookId) return NotFound();

            // Optional: Add custom validation
            if (book.QuantityAvailable > book.TotalQuantity)
            {
                ModelState.AddModelError("QuantityAvailable", "Quantity Available cannot exceed Total Quantity.");
            }

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
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred while saving: {ex.Message}");
                }
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns
            var authorItems = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Select Author --" } };
            authorItems.AddRange(_context.Authors.Select(a => new SelectListItem { Value = a.AuthorId.ToString(), Text = a.Name }));

            var categoryItems = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Select Category --" } };
            categoryItems.AddRange(_context.Categories.Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name }));

            ViewData["Authors"] = new SelectList(authorItems, "Value", "Text", book.AuthorId.ToString());
            ViewData["Categories"] = new SelectList(categoryItems, "Value", "Text", book.CategoryId.ToString());

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