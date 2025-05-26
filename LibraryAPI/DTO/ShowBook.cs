using System.Text.Json.Serialization;

namespace LibraryAPI.DTO;

public class ShowBook
{
    public string Title { get; set; }

    public int Year { get; set; }

    [JsonPropertyName("authorId")]
    public int AuthorId { get; set; }
}