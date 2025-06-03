using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Bookish.Database;
using Microsoft.EntityFrameworkCore;

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

    public IActionResult Books()
    {
        
        return View();
       
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
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
        var users = new List<User>();
        using (var ctx = new BookishContext())
        {
           
            ViewBag.users = await ctx.Users.ToListAsync();
        }      
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
