using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Bookish.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Bookish.Controllers;

public class UserController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly BookishContext _context;

    public UserController(BookishContext context, ILogger<HomeController> logger)
    {   
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Users(IFormCollection data)
    {

        var newUser = new User { Name = data["name"] };
        _context.Users.Add(newUser);
        _context.SaveChanges();
        return RedirectToPage("/Users");
    }

    async public Task<IActionResult> Users()
    {
        ViewBag.users = await _context.Users.ToListAsync();
        return View();
    }

   

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
