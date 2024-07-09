using ChatShuttleX.Data;
using ChatShuttleX.Data.Models;
using ChatShuttleX.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatShuttleX.Tests.Repositories;

public class UserRepoTests
{
    private IUserRepository _userRepository;
    private ChatContext _context;
    
    [OneTimeSetUp]
    public void SetUp()
    {
        var ops = new DbContextOptionsBuilder<ChatContext>()
            .UseInMemoryDatabase(databaseName: "chatshuttlex_test")
            .Options;
        _context = new ChatContext(ops);
        _userRepository = new UserRepository(_context);

        var users = new List<User>
        {
            new() { Id = 1, Username = "user1" },
            new() { Id = 2, Username = "user2" },
            new() { Id = 10, Username = "user10" },
        };
        
        _context.AddRange(users);
        _context.SaveChanges();
    }

    [Test]
    public void GetUserById_UserExists_ReturnsUser()
    {
        var user = _userRepository.GetUserById(1);
        
        Assert.That(user, Is.Not.Null);
        Assert.That(user.Id, Is.EqualTo(1));
    }

    [Test]
    public void GetUserById_UserDoesNotExist_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => _userRepository.GetUserById(3));
        Assert.That(ex.Message, Is.EqualTo("User not found"));
    }
    
    [Test]
    public void GetUserByUsername_UserExists_ReturnsUser()
    {
        var user = _userRepository.GetUserByUsername("user1");
        
        Assert.That(user, Is.Not.Null);
        Assert.That(user.Username, Is.EqualTo("user1"));
    }

    [Test]
    public void GetUserByUsername_UserDoesNotExist_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => _userRepository.GetUserByUsername("nonexistentuser"));
        Assert.That(ex.Message, Is.EqualTo("User not found"));
    }

    [Test]
    public void InsertUser_ValidUser_AddsUserToContext()
    {
        var newUser = new User { Id = 3, Username = "newuser" };
        
        _userRepository.InsertUser(newUser);
        _userRepository.Save();
        
        Assert.That(_context.Users.Find(3), Is.Not.Null);
    }

    [Test]
    public void InsertUser_NullUser_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _userRepository.InsertUser(null));
    }
    
    [Test]
    public void InsertUser_ExistingUser_ThrowsArgumentException()
    {
        var existingUser = new User { Id = 1, Username = "user1" };
        
        var ex = Assert.Throws<ArgumentException>(() => _userRepository.InsertUser(existingUser));
        Assert.That(ex.Message, Is.EqualTo("User already exists"));
    }
    
    [Test]
    public void UpdateUser_ValidUser_UpdatesUserInContext()
    {
        const string newName = "updateduser";
        var updatedUser = new User { Id = 1, Username = newName };
        
        _userRepository.UpdateUser(updatedUser);
        _userRepository.Save();
        
        Assert.That(_context.Users.Any(u => u.Username == newName), Is.True);
    }

    [Test]
    public void UpdateUser_NullUser_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _userRepository.UpdateUser(null));
    }

    [Test]
    public void UpdateUser_UserDoesNotExist_ThrowsArgumentException()
    {
        var nonExistentUser = new User { Id = 50, Username = "nonexistentuser" };

        _userRepository.UpdateUser(nonExistentUser);
        Assert.Throws<DbUpdateConcurrencyException>(() => _userRepository.Save());
    }

    [Test]
    public void DeleteUser_ExistingUser_RemovesUserFromContext()
    {
        var userIdToDelete = 10;
        
        _userRepository.DeleteUser(userIdToDelete);
        _userRepository.Save();
        
        Assert.That(_context.Users.Any(u => u.Id == userIdToDelete), Is.False);
    }

    [Test]
    public void DeleteUser_UserDoesNotExist_ThrowsArgumentException()
    {
        var userIdToDelete = 11;
        
        var ex = Assert.Throws<ArgumentException>(() => _userRepository.DeleteUser(userIdToDelete));
        Assert.That(ex.Message, Is.EqualTo("User not found"));
    }

    [Test]
    public void DeleteUser_InvalidId_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => _userRepository.DeleteUser(-1));
        Assert.That(ex.Message, Is.EqualTo("User not found"));
    }

    
    [OneTimeTearDown]
    public void TearDown()
    {
        _userRepository.Dispose();
        _context.Dispose();
    }
}