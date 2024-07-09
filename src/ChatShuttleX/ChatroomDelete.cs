using System.Text.Json.Serialization;

namespace ChatShuttleX;

public class ChatroomDelete
{
    [JsonPropertyName("chatroom_id")]
    public int ChatroomId { get; set; }
    [JsonPropertyName("username")] 
    public string Username { get; set; }
}