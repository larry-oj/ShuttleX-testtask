using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChatShuttleX;

public class ChatroomSearch
{
    [JsonPropertyName("query")] [Required] public string Query { get; set; }
}