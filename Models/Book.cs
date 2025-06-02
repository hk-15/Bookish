namespace Bookish.Models;

public class Book
{
    public int BookId { get; set; }
    public string? Title { get; set; }
    public int AuthorId { get; set; }
    public Author? Author { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
}
