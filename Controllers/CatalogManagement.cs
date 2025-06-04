using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Bookish.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Bookish.Controllers;

public class CatalogManagementController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BookishContext _context;

    public CatalogManagementController(BookishContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }   

    public IActionResult CatalogManagement()
    {
        return View();
    }

    [HttpPost]
    async public Task<IActionResult> CatalogManagement(IFormCollection data)
    {
        using (var ctx = new BookishContext())
        {
            var authorList = await ctx.Authors.ToListAsync();
            var bookList = await ctx.Books.ToListAsync();

            var authorExists = authorList.Any(author => author.Name!.Equals(data["author"], StringComparison.OrdinalIgnoreCase));
            var bookExists = bookList.Any(book => book.Title!.Equals(data["bookTitle"], StringComparison.OrdinalIgnoreCase));

            if (authorExists)
            {
                if (bookExists)
                {
                    TempData["Message"] = "Book exists in the catalog, please try a new entry. To update details, please go to the Books section.";
                    return RedirectToAction("CatalogManagement");
                }
                else
                {
                    var author = authorList.Find(author => author.Name == data["author"]);
                    _ = int.TryParse(data["copies"], out int copies);
                    var newBook1 = new Book { Title = data["bookTitle"], Author = author, TotalCopies = copies, AvailableCopies = copies };
                    ctx.Books.Add(newBook1);
                    ctx.SaveChanges();
                    TempData["Message"] = "New book added";
                    return RedirectToAction("CatalogManagement");
                }
            }
            else
            {
                _ = int.TryParse(data["copies"], out int copies);
                var newAuthor = new Author { Name = data["author"] };
                ctx.Authors.Add(newAuthor);
                var newBook = new Book { Title = data["bookTitle"], Author = newAuthor, TotalCopies = copies, AvailableCopies = copies };
                ctx.Books.Add(newBook);
                ctx.SaveChanges();
                TempData["Message"] = "New book and author added";
                return RedirectToAction("CatalogManagement");
            }
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
