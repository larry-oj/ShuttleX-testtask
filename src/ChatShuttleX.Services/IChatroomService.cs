using ChatShuttleX.Services.Models;

namespace ChatShuttleX.Services;

public interface IChatroomService : IDisposable
{
    /// <summary>
    /// Create a new chatroom
    /// </summary>
    /// <param name="chatroomName">Name of a chatroom</param>
    /// <param name="creatorId">Unique identifier of the crator</param>
    /// <returns><see cref="ChatroomModel"/> of a new chat</returns>
    void CreateChatroom(string chatroomName, int creatorId);
    /// <summary>
    /// Create a new chatroom
    /// </summary>
    /// <param name="chatroomName">Name of a chatroom</param>
    /// <param name="creatorUsername">Unique name of the crator</param>
    void CreateChatroom(string chatroomName, string creatorUsername);
    /// <summary>
    /// Get all chatrooms with a given name
    /// </summary>
    /// <param name="name">Name of a chatroom (not case-sensitive)</param>
    /// <returns><see cref="IEnumerable{T}"/>(of <see cref="ChatroomModel"/>) of found chats</returns>
    IEnumerable<ChatroomModel> GetChatrooms(string name = "");
    /// <summary>
    /// Get chatroom by unique identifier
    /// </summary>
    /// <param name="chatroomId">Unique identifier</param>
    /// <returns><see cref="ChatroomModel"/> of found chatroom</returns>
    ChatroomModel GetChatroom(int chatroomId);
    /// <summary>
    /// Delete chatroom from the system.
    /// </summary>
    /// <param name="chatroomId">Unique identifier</param>
    void DeleteChatroom(int chatroomId);
}