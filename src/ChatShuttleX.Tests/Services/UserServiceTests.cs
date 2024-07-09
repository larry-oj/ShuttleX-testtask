using ChatShuttleX.Data;
using ChatShuttleX.Data.Models;
using ChatShuttleX.Data.Repositories;
using ChatShuttleX.Services;
using ChatShuttleX.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ChatShuttleX.Tests.Services;

public class UserServiceTests
{
    private ChatContext _context;
    private IUserRepository _userRepository;
    private IUserService _userService;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ChatContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique name for each test to ensure isolation
            .Options;

        _context = new ChatContext(options);
        _userRepository = new UserRepository(_context);
        _userService = new UserService(_userRepository);

        var users = new[]
        {
            new User { Id = 1, Username = "user1" },
            new User { Id = 2, Username = "user2" }
        };

        _context.Users.AddRange(users);
        _context.SaveChanges();
    }
    
    [Test]
    public void Register_ValidUsername_SuccessfullyRegisters()
    {
        // Arrange
        string username = "testuser";

        // Act
        _userService.Register(username);

        // Assert
        var result = _userService.GetUser(username);
        Assert.IsNotNull(result);
        Assert.AreEqual(username, result.Username);
    }

    [Test]
    public void Register_DuplicateUsername_ThrowsUserAlreadyExistsException()
    {
        // Arrange
        string existingUsername = "user1";

        // Act & Assert
        Assert.Throws<UserAlreadyExistsException>(() => _userService.Register(existingUsername));
    }

    [Test]
    public void GetUser_ValidUsername_ReturnsUserModel()
    {
        // Arrange
        string username = "user1";

        // Act
        var result = _userService.GetUser(username);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(username, result.Username);
    }

    [Test]
    public void GetUser_InvalidUsername_ThrowsUserDoesNotExistException()
    {
        // Arrange
        string nonExistingUsername = "nonexistinguser";

        // Act & Assert
        Assert.Throws<UserDoesNotExistException>(() => _userService.GetUser(nonExistingUsername));
    }

    [Test]
    public void DeleteUser_ValidUsername_SuccessfullyDeletes()
    {
        // Arrange
        string username = "user1";

        // Act
        _userService.DeleteUser(username);

        // Assert
        Assert.Throws<UserDoesNotExistException>(() => _userService.GetUser(username));
    }

    [Test]
    public void DeleteUser_InvalidUsername_ThrowsUserDoesNotExistException()
    {
        // Arrange
        string nonExistingUsername = "nonexistinguser";

        // Act & Assert
        Assert.Throws<UserDoesNotExistException>(() => _userService.DeleteUser(nonExistingUsername));
    }

    [Test]
    public void DeleteUser_ValidUserId_SuccessfullyDeletes()
    {
        // Arrange
        int userId = 1;

        // Act
        _userService.DeleteUser(userId);

        // Assert
        Assert.Throws<UserDoesNotExistException>(() => _userService.GetUser(userId));
    }

    [Test]
    public void DeleteUser_InvalidUserId_ThrowsUserDoesNotExistException()
    {
        // Arrange
        int nonExistingUserId = 999;

        // Act & Assert
        Assert.Throws<UserDoesNotExistException>(() => _userService.DeleteUser(nonExistingUserId));
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        _userRepository.Dispose();
        _userService.Dispose();
    }
}