using ChatShuttleX.Data.Models;
using ChatShuttleX.Data.Repositories;
using ChatShuttleX.Services.Exceptions;
using ChatShuttleX.Services.Models;

namespace ChatShuttleX.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public void Register(string username)
    {
        try
        {
            userRepository.InsertUser(new User {Username = "username"});
        }
        catch (Exception e)
        {
            throw e switch
            {
                ArgumentException { Message: "User already exists" } 
                    => new UserAlreadyExistsException(),
                ArgumentNullException 
                    => new InvalidUsernameException(),
                _ 
                    => new Exception(e.Message, e)
            };
        }
        userRepository.Save();
    }

    public bool UserExists(string username)
    {
        try
        {
            userRepository.GetUserByUsername(username);
            return true;
        }
        catch (Exception e)
        {
            if (e is not ArgumentException { Message: "User not found" })
            {
                throw new Exception(e.Message, e);
            }
        }

        return false;
    }

    public UserModel GetUser(string username)
    {
        try
        {
            return new UserModel(userRepository.GetUserByUsername(username));
        }
        catch (Exception e)
        {
            throw e switch
            {
                ArgumentException { Message: "User not found" } 
                    => new UserDoesNotExistException(),
                _
                    => new Exception(e.Message, e)
            };
        }
    }

    public UserModel GetUser(int userId)
    {
        try
        {
            return new UserModel(userRepository.GetUserById(userId));
        }
        catch (Exception e)
        {
            throw e switch
            {
                ArgumentException { Message: "User not found" } 
                    => new UserDoesNotExistException(),
                _
                    => new Exception(e.Message, e)
            };
        }
    }

    public void DeleteUser(string username)
    {
        try
        {
            userRepository.DeleteUser(userRepository.GetUserByUsername(username).Id);
            userRepository.Save();
        }
        catch (Exception e)
        {
            throw e switch
            {
                ArgumentException { Message: "User not found" } 
                    => new UserDoesNotExistException(),
                _
                    => new Exception(e.Message, e)
            };
        }
    }

    public void DeleteUser(int userId)
    {
        try
        {
            userRepository.DeleteUser(userId);
            userRepository.Save();
        }
        catch (Exception e)
        {
            throw e switch
            {
                ArgumentException { Message: "User not found" } 
                    => new UserDoesNotExistException(),
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
                userRepository.Dispose();
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