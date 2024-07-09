using ChatShuttleX.Data.Models;

namespace ChatShuttleX.Data.Repositories;

public interface IChatroomRepository : IDisposable
{
    IEnumerable<Chatroom> GetChatrooms();
    Chatroom GetChatroomById(int id);
    Chatroom GetChatroomByName(string name);
    Chatroom GetChatroomByOwner(User owner);
    void InsertChatroom(Chatroom chatroom);
    void UpdateChatroom(Chatroom chatroom);
    void DeleteChatroom(int id);
    void Save();
    Task SaveAsync();
}