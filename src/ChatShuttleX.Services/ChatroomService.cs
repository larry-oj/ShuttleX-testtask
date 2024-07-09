using ChatShuttleX.Data.Models;
using ChatShuttleX.Data.Repositories;
using ChatShuttleX.Services.Exceptions;
using ChatShuttleX.Services.Models;

namespace ChatShuttleX.Services;

public class ChatroomService(IChatroomRepository chatroomRepository) : IChatroomService
{
    public void CreateChatroom(string chatroomName, int creatorId)
    {
        try
        {
            chatroomRepository.InsertChatroom(new Chatroom { Name = chatroomName, Creator = new User { Id = creatorId } });
        }
        catch (Exception e)
        {
            throw e switch
            {
                ArgumentNullException 
                    => new InvalidChatroomNameException(),
                ArgumentException { Message: "Chatroom already exists" } 
                    => new ChatroomAlreadyExistsException(),
                ArgumentException { Message: "User doesn't exist" } 
                    => new UserDoesNotExistException(),
                _
                    => new Exception(e.Message, e)
            };
        }
    }

    public void CreateChatroom(string chatroomName, string creatorUsername)
    {
        try
        {
            chatroomRepository.InsertChatroom(new Chatroom { Name = chatroomName, Creator = new User { Username = creatorUsername } });
        }
        catch (Exception e)
        {
            throw e switch
            {
                ArgumentNullException 
                    => new InvalidChatroomNameException(),
                ArgumentException { Message: "Chatroom already exists" } 
                    => new ChatroomAlreadyExistsException(),
                ArgumentException { Message: "User doesn't exist" } 
                    => new UserDoesNotExistException(),
                _
                    => new Exception(e.Message, e)
            };
        }
    }

    public IEnumerable<ChatroomModel> GetChatrooms(string name = "")
    {
        try
        {
            return name switch
            {
                "" => chatroomRepository.GetChatrooms().Select(c => new ChatroomModel(c)),
                _ => chatroomRepository.GetAllChatroomsByName(name).Select(c => new ChatroomModel(c))
            };
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public void DeleteChatroom(int chatroomId)
    {
        try
        {
            chatroomRepository.DeleteChatroom(chatroomId);
        }
        catch (Exception e)
        {
            throw e switch
            {
                ArgumentException { Message: "Chatroom not found" } 
                    => new ChatroomDoesNotExistException(),
                _
                    => new Exception(e.Message, e)
            };
        }
    }
    
    // IDisposable 
    private bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                chatroomRepository.Dispose();
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