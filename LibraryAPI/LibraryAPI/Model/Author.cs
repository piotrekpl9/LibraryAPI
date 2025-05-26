using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Model;

public class Author
{
    public int Id { get; set; }
    [MaxLength(255)]
    public string FirstName { get; set; }
    [MaxLength(255)]
    public string LastName { get; set; }
    
}