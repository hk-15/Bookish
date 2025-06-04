using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Bookish.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Bookish.Controllers;

public class BookController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    private readonly BookishContext _context;

    public BookController(BookishContext context, ILogger<HomeController> logger)
    {
        _logger = logger;
        _context = context;
    }

    async public Task<IActionResult> Books()
    {
        ViewBag.books = await _context.Books.Include(b => b.Author).ToListAsync();
        return View();
    }

    async public Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var book = await _context.Books.FindAsync(id);            
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }


    // [HttpPatch("{id}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,TotalCopies")] Book book)
    {
        if (id != book.BookId)
        {
            return NotFound();
        }
        
        var bookList = await _context.Books.AsNoTracking().ToListAsync();
        var bookExists = bookList.Any(book => book.BookId!.Equals(id));

        var authorId = from b in _context.Books
                       where b.BookId == id
                       select b.AuthorId;
        var currentAvailableCopies = from b in _context.Books
                       where b.BookId == id
                       select b.AvailableCopies;
        var currentTotalCopies = from b in _context.Books
                       where b.BookId == id
                       select b.TotalCopies;

        var newAvailableCopies = book.TotalCopies - currentTotalCopies.First() + currentAvailableCopies.First();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(book);
                book.AuthorId = authorId.First();
                book.AvailableCopies = newAvailableCopies;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!bookExists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Books");
        }
        return View(book);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
