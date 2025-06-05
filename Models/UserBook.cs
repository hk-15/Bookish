namespace Bookish.Models;

public class UserBook
{
    [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
    public int UserBookId { get; set; }

    public int BookId { get; set; }

    public Book? Book { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }

}