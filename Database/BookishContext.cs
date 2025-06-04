using Bookish.Models;
using Microsoft.EntityFrameworkCore;
namespace Bookish.Database

{
    public class BookishContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }
        // public BookishContext(DbContextOptions<BookishContext> options)
        // : base(options)
        // {}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql(@"Server=localhost;Port=5432;Database=bookish;User Id=bookish;Password=bookish;")
                .EnableSensitiveDataLogging();
        }
        
    }
}