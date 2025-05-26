using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryAPI.DTO;

public class CreateAuthor
{
    [MaxLength(255)]
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }
    [MaxLength(255)]
    [JsonPropertyName("last_name")]
    public string LastName { get; set; }
}