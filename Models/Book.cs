namespace Bookish.Models;

public class Book
{
    [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
    public int BookId { get; set; }
    public string? Title { get; set; }
    public int AuthorId { get; set; }
    public Author? Author { get; set; }

    // [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
}
