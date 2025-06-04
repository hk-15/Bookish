// using System.Diagnostics;
// using Microsoft.AspNetCore.Mvc;
// using Bookish.Models;
// using Bookish.Database;
// using Microsoft.EntityFrameworkCore;
// using System.Threading.Tasks;
// using System.Linq;

// namespace Bookish.Controllers;

// public class UserController : Controller
// {
//     private readonly ILogger<HomeController> _logger;

//     public UserController(ILogger<HomeController> logger)
//     {
//         _logger = logger;
//     }

//     [HttpPost]
//     public IActionResult Users(IFormCollection data)
//     {

//         using (var ctx = new BookishContext())
//         {
//             var newUser = new User { Name = data["name"] };
//             ctx.Users.Add(newUser);
//             ctx.SaveChanges();
//         }
//         return RedirectToPage("/Users");
//     }

//     async public Task<IActionResult> Users()
//     {
//         using (var ctx = new BookishContext())
//         {
//             ViewBag.users = await ctx.Users.ToListAsync();
//         }
//         return View();
//     }

   

//     [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//     public IActionResult Error()
//     {
//         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//     }
// }
