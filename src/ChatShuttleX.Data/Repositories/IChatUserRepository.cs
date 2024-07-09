using ChatShuttleX.Data.Models;

namespace ChatShuttleX.Data.Repositories;

public interface IChatUserRepository
{
    IEnumerable<ChatUser> GetChatUsers();
    ChatUser GetChatUserById(int id);
    ChatUser GetChatUserByUser(User user);
    ChatUser GetChatUserByChatroom(Chatroom owner);
    void InsertChatUser(ChatUser chatUser);
    void UpdateChatUser(ChatUser chatUser);
    void DeleteChatUser(int id);
    void Save();
    Task SaveAsync();
}