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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,TotalCopies")] Book newBook)
    {
        if (id != newBook.BookId)
        {
            return NotFound();
        }
        
        var oldBook = _context.Books.AsNoTracking().Single(book => book.BookId == id);
        var newAvailableCopies = newBook.TotalCopies - oldBook.TotalCopies + oldBook.AvailableCopies;

        if (ModelState.IsValid)
        {
            try
            {
                newBook.AuthorId = oldBook.AuthorId;
                newBook.AvailableCopies = newAvailableCopies;
                _context.Update(newBook);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (oldBook != null)
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
        return View(newBook);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        TempData["Message"] = "Are you sure you want to delete?";
        var book = await _context.Books
            .FirstOrDefaultAsync(b => b.BookId == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction("Books");
    }


     public async Task<IActionResult> Borrow(int? id)
    {
        
        if (id == null)
        {
            return NotFound();
        }
        var book = await _context.Books
            .FirstOrDefaultAsync(b => b.BookId == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrow(int bookId, int UserId)
    {
        var book = _context.Books.AsNoTracking().Single(book => book.BookId == bookId);
        var newAvailableCopies = book.AvailableCopies - 1;
        var enteredUser = await _context.Users.FirstOrDefaultAsync(i => i.UserId == UserId);
        
        if (ModelState.IsValid)
        {
            try
            {
                book.AuthorId = book.AuthorId;
                book.Title = book.Title;
                book.AvailableCopies = newAvailableCopies;
                if (enteredUser == null)
                {
                    TempData["Message"] = "Please enter a user that is registered with the library";
                    return RedirectToAction("Borrow");
                }
                else
                {
                    _context.Update(book);
                    var ubook = new UserBook {Book = book, User = enteredUser};
                    _context.Update(ubook);                    
                    await _context.SaveChangesAsync();
                }
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (book != null)
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
        return RedirectToAction("Books");

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
