namespace ChatShuttleX.Services.Models;

/// <summary>
/// Represents a user in the system
/// </summary>
public class UserModel
{
    /// <summary>
    /// Unique identifier of the user
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Unique username of the user
    /// </summary>
    public string Username { get; set; }
}