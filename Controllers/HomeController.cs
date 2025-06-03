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

    public async Task<IActionResult> CatalogManagement(IFormCollection data)
    {

        using (var ctx = new BookishContext())
        {
            var authorList = await ctx.Authors.ToListAsync();            
            var bookList = await ctx.Books.ToListAsync();
            
            var authorExists = authorList.Any(author => author.Name == data["author"]);
            var bookExists = bookList.Any(book => book.Title == data["bookTitle"]);
            //book.author = authorExists ? book.author : data["author"]
            

            foreach (var author in authorList)
            {
                if (author.Name == data["author"])
                {
                    _ = int.TryParse(data["copies"], out int copies);
                    var newBook1 = new Book { Title = data["bookTitle"], Author = author, TotalCopies = copies, AvailableCopies = copies };
                    ctx.Books.Add(newBook1);
                    ctx.SaveChanges();
                }
                else
                {
                    _ = int.TryParse(data["copies"], out int copies);
                    var newAuthor = new Author { Name = data["author"] };
                    ctx.Authors.Add(newAuthor);
                    var newBook = new Book { Title = data["bookTitle"], Author = newAuthor, TotalCopies = copies, AvailableCopies = copies };
                    ctx.Books.Add(newBook);
                    ctx.SaveChanges();
                }
            }
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
