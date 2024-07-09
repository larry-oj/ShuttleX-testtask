using ChatShuttleX.Data.Models;

namespace ChatShuttleX.Data.Repositories;

public class ChatroomRepository(ChatContext context) : IChatroomRepository
{
    public IEnumerable<Chatroom> GetChatrooms()
        => context.Chatrooms.AsEnumerable();

    public Chatroom GetChatroomById(int id)
    {
        var chatroom = context.Chatrooms.Find(id);
        if (chatroom == null)
            throw new ArgumentException("Chatroom not found");
        return chatroom;
    }

    public Chatroom GetChatroomByName(string name)
    {
        var chatroom = context.Chatrooms.FirstOrDefault(c => c.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        if (chatroom == null)
            throw new ArgumentException("Chatroom not found");
        return chatroom;
    }

    public IEnumerable<Chatroom> GetAllChatroomsByName(string name)
    {
        var chatroom = context.Chatrooms.Where(c => c.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        return chatroom;
    }

    public Chatroom GetChatroomByOwner(User owner)
    {
        var chatroom = context.Chatrooms.FirstOrDefault(c => c.Creator == owner);
        if (chatroom == null)
            throw new ArgumentException("Chatroom not found");
        return chatroom;
    }

    public void InsertChatroom(Chatroom chatroom)
    {
        ArgumentNullException.ThrowIfNull(chatroom);

        if (context.Chatrooms.Any(c => c.Id == chatroom.Id || c.Name.Equals(chatroom.Name, StringComparison.CurrentCultureIgnoreCase)))
            throw new ArgumentException("Chatroom already exists");

        if (!context.Users.Any(u => u.Id == chatroom.Creator.Id))
            throw new ArgumentException("User doesn't exist");

        context.Chatrooms.Add(chatroom);
    }

    public void UpdateChatroom(Chatroom chatroom)
    {
        ArgumentNullException.ThrowIfNull(chatroom);
        
        if (!context.Chatrooms.Any(u => u.Id == chatroom.Id))
            throw new ArgumentException("Chatroom doesn't exist");
        
        context.Chatrooms.Update(chatroom);
    }

    public void DeleteChatroom(int id)
    {
        var chatToDelete = context.Chatrooms.Find(id);
        if (chatToDelete == null)
            throw new ArgumentException("Chatroom not found");

        context.Chatrooms.Remove(chatToDelete);
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