using ChatShuttleX.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatShuttleX.Data.Repositories;

public class ChatUserRepository(ChatContext context) : IChatUserRepository
{
    public IEnumerable<ChatUser> GetChatUsers()
        => context.ChatUsers.AsEnumerable();

    public IEnumerable<ChatUser> GetChatUsersByChatroomId(int chatroomId)
    {
        var chatusers = context.ChatUsers.Where(cu => cu.Chatroom.Id == chatroomId);
        if (!chatusers.Any())
            throw new ArgumentException("Chatroom not found");
        return chatusers.AsEnumerable();
    }

    public IEnumerable<ChatUser> GetChatUsersByChatroom(Chatroom chat)
    {
        var chatusers = context.ChatUsers.Where(cu => cu.Chatroom == chat);
        if (!chatusers.Any())
            throw new ArgumentException("Chatroom not found");
        return chatusers.AsEnumerable();
    }

    public void InsertChatUser(ChatUser chatroom)
    {
        ArgumentNullException.ThrowIfNull(chatroom);
        
        if (context.ChatUsers.Any(cu => cu.User.Id == chatroom.User.Id && cu.Chatroom.Id == chatroom.Chatroom.Id))
            throw new ArgumentException("ChatUser already exists");
        
        context.ChatUsers.Add(chatroom);
    }

    public void UpdateChatUser(ChatUser chatroom)
    {
        ArgumentNullException.ThrowIfNull(chatroom);
        
        if (!context.ChatUsers.Any(cu => cu.Id == chatroom.Id))
            throw new ArgumentException("ChatUser doesn't exist");
        
        context.ChatUsers.Update(chatroom);
    }

    public void DeleteChatUser(int id)
    {
        var chatuserToDelete = context.ChatUsers.Find(id);
        if (chatuserToDelete == null)
            throw new ArgumentException("ChatUser not found");

        context.ChatUsers.Remove(chatuserToDelete);
    }

    public void Save()
    {
        context.SaveChanges();
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
    
    // IDisposable 
    private bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}