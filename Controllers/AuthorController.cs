
    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice_Project.Data;
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

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            var authors = await _context.Authors.ToListAsync();
            return View(authors);
        }

      

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Authors authors)
        {
            if (ModelState.IsValid)
            {
                _context.Add(authors);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(authors);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            return View(author);
        }

        // POST: Authors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Authors authors)
        {
            if (id != authors.AuthorId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(authors);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(authors.AuthorId))
                        return NotFound();

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(authors);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var author = await _context.Authors
                .FirstOrDefaultAsync(a => a.AuthorId == id);

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
            return _context.Authors.Any(a => a.AuthorId == id);
        }
    }
}

