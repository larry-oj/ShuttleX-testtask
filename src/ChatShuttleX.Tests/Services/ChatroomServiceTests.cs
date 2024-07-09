using ChatShuttleX.Data;
using ChatShuttleX.Data.Models;
using ChatShuttleX.Data.Repositories;
using ChatShuttleX.Services;
using ChatShuttleX.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ChatShuttleX.Tests.Services;

public class ChatroomServiceTests
{
    private ChatContext _context;
    private IChatroomRepository _chatroomRepository;
    private IChatroomService _chatroomService;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ChatContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ChatContext(options);
        _chatroomRepository = new ChatroomRepository(_context);
        _chatroomService = new ChatroomService(_chatroomRepository);

        SeedTestData();
    }

    private void SeedTestData()
    {
        var users = new[]
        {
            new User { Id = 1, Username = "user1" },
            new User { Id = 2, Username = "user2" }
        };

        var chatrooms = new[]
        {
            new Chatroom { Id = 1, Name = "chatroom1", Creator = users[0] }
        };

        _context.Users.AddRange(users);
        _context.Chatrooms.AddRange(chatrooms);
        _context.SaveChanges();
    }

    [Test]
    public void CreateChatroom_ValidNameAndCreatorId_SuccessfullyCreates()
    {
        // Arrange
        string chatroomName = "newchatroom";
        int creatorId = 2; // user2

        // Act
        _chatroomService.CreateChatroom(chatroomName, creatorId);

        // Assert
        var result = _chatroomService.GetChatrooms().FirstOrDefault(c => c.Name == chatroomName);
        Assert.IsNotNull(result);
        Assert.AreEqual(chatroomName, result.Name);
    }

    [Test]
    public void CreateChatroom_DuplicateName_ThrowsChatroomAlreadyExistsException()
    {
        // Arrange
        string existingChatroomName = "chatroom1";
        int creatorId = 1; // user1

        // Act & Assert
        Assert.Throws<ChatroomAlreadyExistsException>(() => _chatroomService.CreateChatroom(existingChatroomName, creatorId));
    }

    [Test]
    public void CreateChatroom_InvalidCreatorId_ThrowsUserDoesNotExistException()
    {
        // Arrange
        string chatroomName = "newchatroom";
        int nonExistingCreatorId = 999;

        // Act & Assert
        Assert.Throws<UserDoesNotExistException>(() => _chatroomService.CreateChatroom(chatroomName, nonExistingCreatorId));
    }

    [Test]
    public void GetChatrooms_NoNameFilter_ReturnsAllChatrooms()
    {
        // Act
        var result = _chatroomService.GetChatrooms();

        // Assert
        Assert.AreEqual(1, result.Count()); // One chatroom exists in the seeded data
    }

    [Test]
    public void GetChatrooms_WithNameFilter_ReturnsFilteredChatrooms()
    {
        // Arrange
        string chatroomNameFilter = "chatroom1";

        // Act
        var result = _chatroomService.GetChatrooms(chatroomNameFilter);

        // Assert
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual(chatroomNameFilter, result.First().Name);
    }

    [Test]
    public void DeleteChatroom_ValidChatroomId_SuccessfullyDeletes()
    {
        // Arrange
        int chatroomIdToDelete = 1; // chatroom1

        // Act
        _chatroomService.DeleteChatroom(chatroomIdToDelete);

        // Assert
        Assert.Throws<ChatroomDoesNotExistException>(() => _chatroomService.GetChatrooms().FirstOrDefault(c => c.Id == chatroomIdToDelete));
    }

    [Test]
    public void DeleteChatroom_InvalidChatroomId_ThrowsChatroomDoesNotExistException()
    {
        // Arrange
        int nonExistingChatroomId = 999;

        // Act & Assert
        Assert.Throws<ChatroomDoesNotExistException>(() => _chatroomService.DeleteChatroom(nonExistingChatroomId));
    }
    
    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        _chatroomRepository.Dispose();
        _chatroomService.Dispose();
    }
}