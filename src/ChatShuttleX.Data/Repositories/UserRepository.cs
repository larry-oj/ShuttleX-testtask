using ChatShuttleX.Data.Models;

namespace ChatShuttleX.Data.Repositories;

public class UserRepository(ChatContext context) : IUserRepository
{
    public IEnumerable<User> GetUsers()
        => context.Users.AsEnumerable();

    public User GetUserById(int id)
    {
        var user = context.Users.Find(id);
        if (user == null)
            throw new ArgumentException("User not found");
        return user;
    }

    public User GetUserByUsername(string username)
    {
        var user = context.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase));
        if (user == null)
            throw new ArgumentException("User not found");
        return user;
    }

    public void InsertUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (context.Users.Any(u => u.Id == user.Id || u.Username == user.Username))
            throw new ArgumentException("User already exists");

        context.Users.Add(user);
    }
    

    public void UpdateUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        if (!context.Users.Any(u => u.Id == user.Id))
            throw new ArgumentException("User doesn't exist");
        
        context.Users.Update(user);
    }

    public void DeleteUser(int id)
    {
        var userToDelete = context.Users.Find(id);
        if (userToDelete == null)
            throw new ArgumentException("User not found");

        context.Users.Remove(userToDelete);
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