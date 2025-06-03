namespace Bookish.Models;

public class User()
{
    [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }
    public string? Name { get; set; }
}