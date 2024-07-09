using ChatShuttleX.Data.Models;
using ChatShuttleX.Data.Repositories;
using ChatShuttleX.Services.Models;

namespace ChatShuttleX.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public UserModel Register(string username)
    {
        throw new NotImplementedException();
    }

    public UserModel GetUser(string username)
    {
        throw new NotImplementedException();
    }

    public UserModel GetUser(int userId)
    {
        throw new NotImplementedException();
    }

    public void DeleteUser(string username)
    {
        throw new NotImplementedException();
    }

    public void DeleteUser(int userId)
    {
        throw new NotImplementedException();
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