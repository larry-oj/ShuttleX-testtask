using ChatShuttleX.Data.Models;

namespace ChatShuttleX.Data.Repositories;

public interface IChatUserRepository : IDisposable
{
    IEnumerable<ChatUser> GetChatUsers();
    IEnumerable<ChatUser> GetChatUsersByChatroomId(int id);
    IEnumerable<ChatUser> GetChatUsersByChatroom(Chatroom chat);
    void InsertChatUser(ChatUser chatUser);
    void UpdateChatUser(ChatUser chatUser);
    void DeleteChatUser(int id);
    void Save();
    Task SaveAsync();
}