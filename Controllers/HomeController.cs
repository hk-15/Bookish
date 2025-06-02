using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Bookish.Database;

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
        // using (var ctx = new BookishContext()) {
        // var newBook = new Book();
        return View();
        // }
    }

        public IActionResult Users()
    {
        using (var ctx = new BookishContext() ) {
        // ctx.Database.EnsureCreated();
        var newUser = new User{Id=2, Name="testuser2"};
        ctx.Users.Add(newUser);
        ctx.SaveChanges();
        
        return View();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
