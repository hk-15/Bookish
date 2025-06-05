namespace Bookish.Models;

public class User
{
  public User()
  {
    UserBooks = new List<UserBook>();
  }
  
  [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]

  public int UserId { get; set; }
  public string? Name { get; set; }
  
  public List<UserBook>? UserBooks { get; set; }
   
}