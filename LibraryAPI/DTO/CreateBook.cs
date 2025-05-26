using System.Text.Json.Serialization;

namespace LibraryAPI.DTO;

public class CreateBook
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("authorId")]
    public int AuthorId { get; set; }
}