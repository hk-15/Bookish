using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Bookish.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Bookish.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    async public Task<IActionResult> Books()
    {
        using (var ctx = new BookishContext())
        {
            ViewBag.books = await ctx.Books.Include(b => b.Author).ToListAsync();
        }

        return View();

    }

    [HttpPost]
    public IActionResult Users(IFormCollection data)
    {

        using (var ctx = new BookishContext())
        {
            var newUser = new User { Name = data["name"] };
            ctx.Users.Add(newUser);
            ctx.SaveChanges();
        }
        return RedirectToPage("/Users");
    }

    async public Task<IActionResult> Users()
    {
        using (var ctx = new BookishContext())
        {
            ViewBag.users = await ctx.Users.ToListAsync();
        }
        return View();
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

            // var authorExists = authorList.Any(author => author.Name == data["author"]);
            var authorExists = authorList.Any(author => author.Name.Equals(data["author"], StringComparison.OrdinalIgnoreCase));
            // var bookExists = bookList.Any(book => book.Title == data["bookTitle"]);
            var bookExists = bookList.Any(book => book.Title.Equals(data["bookTitle"], StringComparison.OrdinalIgnoreCase));
            // book.author = authorExists ? book.author : data["author"];

            if (authorExists)
            {
                if (bookExists)
                {

                    TempData["Message"] = "Book exists in the catalog, please try a new entry";
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


    // foreach (var author in authorList)
    // {
    //     if (author.Name == data["author"])
    //     {
    //         _ = int.TryParse(data["copies"], out int copies);
    //         var newBook1 = new Book { Title = data["bookTitle"], Author = author, TotalCopies = copies, AvailableCopies = copies };
    //         ctx.Books.Add(newBook1);
    //         ctx.SaveChanges();
    //     }
    //     else
    //     {
    //         _ = int.TryParse(data["copies"], out int copies);
    //         var newAuthor = new Author { Name = data["author"] };
    //         ctx.Authors.Add(newAuthor);
    //         var newBook = new Book { Title = data["bookTitle"], Author = newAuthor, TotalCopies = copies, AvailableCopies = copies };
    //         ctx.Books.Add(newBook);
    //         ctx.SaveChanges();
    //     }
    // }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
