using ChatShuttleX.Data.Models;

namespace ChatShuttleX.Data.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetUsers();
    User GetUserById(int id);
    User GetUserByUsername(string username);
    void InsertUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int id);
    void Save();
    Task SaveAsync();
}