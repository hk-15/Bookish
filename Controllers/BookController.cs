using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Bookish.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Bookish.Controllers;

public class BookController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    private readonly BookishContext _context;

    public BookController(BookishContext context, ILogger<HomeController> logger) {
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
            var bookWithAuthor = _context.Books.Include(b => b.Author);
            

            if (bookWithAuthor == null)
        {
            return NotFound();
        }
        return View(bookWithAuthor);
       
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,TotalCopies")] Book book)
    {
        if (id != book.BookId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var bookExists = _context.Books.Include(b => b.Author).Any(book => book.BookId!.Equals(id));
               try
                {
                    
                    _context.Update(book);
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
