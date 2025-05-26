using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Model;

public class Book
{
    public int Id { get; set; }
    [MaxLength(255)]
    public string Title { get; set; }
    public int Year { get; set; }
    public int AuthorId { get; set; }
    public virtual Author Author { get; set; }
}