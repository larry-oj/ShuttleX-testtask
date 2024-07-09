using ChatShuttleX.Services.Models;

namespace ChatShuttleX.Services;

public interface IUserService : IDisposable
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="username">Unique name</param>
    void Register(string username);
    /// <summary>
    /// Get user by username
    /// </summary>
    /// <param name="username">Unique name (non case-sensitive)</param>
    /// <returns><see cref="UserModel"/> of a found user</returns>
    UserModel GetUser(string username);
    /// <summary>
    /// Get user by username
    /// </summary>
    /// <param name="userId">Unique identifier</param>
    /// <returns><see cref="UserModel"/> of a found user</returns>
    UserModel GetUser(int userId);
    /// <summary>
    /// Delete user from the system.
    /// </summary>
    /// <param name="username">Unique name</param>
    void DeleteUser(string username);
    /// <summary>
    /// Delete user from the system.
    /// </summary>
    /// <param name="userId">Unique identifier</param>
    void DeleteUser(int userId);
}