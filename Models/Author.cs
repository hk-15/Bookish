namespace Bookish.Models;

public class Author
{
    public Author()
    {
        Books = new List<Book>();
    }
    
    [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
    public int AuthorId { get; set; }
    public string? Name { get; set; }

    public IList<Book>? Books { get; set; }
}