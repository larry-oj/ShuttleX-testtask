using ChatShuttleX.Data.Models;

namespace ChatShuttleX.Services.Models;

/// <summary>
/// Represents a chatroom in the system 
/// </summary>
public class ChatroomModel
{
    /// <summary>
    /// The unique identifier of the chatroom
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Name of the chatroom. Must be unique
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Chatroom creator
    /// </summary>
    public UserModel Owner { get; set; }

    public ChatroomModel()
    {
        
    }

    public ChatroomModel(Chatroom room)
    {
        Id = room.Id;
        Name = room.Name;
        Owner = new UserModel(room.Creator);
    }
}