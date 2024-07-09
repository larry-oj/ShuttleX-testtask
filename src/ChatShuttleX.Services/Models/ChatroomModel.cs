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
    public int Name { get; set; }
    /// <summary>
    /// Chatroom creator
    /// </summary>
    public UserModel Owner { get; set; }
    /// <summary>
    /// Chatroom participants. Includes creator
    /// </summary>
    public List<UserModel> Participants { get; set; }
}