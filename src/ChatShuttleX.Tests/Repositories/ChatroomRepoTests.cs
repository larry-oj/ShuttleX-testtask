using ChatShuttleX.Data;
using ChatShuttleX.Data.Models;
using ChatShuttleX.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatShuttleX.Tests.Repositories;

public class ChatroomRepoTests
{
    private IChatroomRepository _chatroomRepository;
    private ChatContext _context;
    
    [OneTimeSetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ChatContext>()
            .UseInMemoryDatabase(databaseName: "chatshuttlex_test")
            .Options;
        
        _context = new ChatContext(options);
        _chatroomRepository = new ChatroomRepository(_context);

        var chatrooms = new List<Chatroom>
        {
            new Chatroom { Id = 1, Name = "Chatroom1", Creator = new User { Id = 1, Username = "user1" } },
            new Chatroom { Id = 2, Name = "Chatroom2", Creator = new User { Id = 2, Username = "user2" } },
            new Chatroom { Id = 10, Name = "Chatroom10", Creator = new User { Id = 10, Username = "user10" } },
        };
        
        _context.AddRange(chatrooms);
        _context.SaveChanges();
    }

    [Test]
    public void GetChatrooms_ReturnsAllChatrooms()
    {
        var chatrooms = _chatroomRepository.GetChatrooms();

        Assert.That(chatrooms.Count(), Is.EqualTo(3));
    }

    [Test]
    public void GetChatroomById_ValidId_ReturnsChatroom()
    {
        var chatroom = _chatroomRepository.GetChatroomById(1);

        Assert.That(chatroom, Is.Not.Null);
        Assert.That(chatroom.Name, Is.EqualTo("Chatroom1"));
    }

    [Test]
    public void GetChatroomById_InvalidId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _chatroomRepository.GetChatroomById(999));
    }

    [Test]
    public void GetChatroomByName_ValidName_ReturnsChatroom()
    {
        var chatroom = _chatroomRepository.GetChatroomByName("Chatroom2");

        Assert.That(chatroom, Is.Not.Null);
        Assert.That(chatroom.Id, Is.EqualTo(2));
    }

    [Test]
    public void GetChatroomByName_InvalidName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _chatroomRepository.GetChatroomByName("NonExistentRoom"));
    }

    [Test]
    public void InsertChatroom_ValidChatroom_InsertsSuccessfully()
    {
        var newChatroom = new Chatroom { Id = 20, Name = "NewChatroom", Creator = new User { Id = 1, Username = "user1" } };

        _chatroomRepository.InsertChatroom(newChatroom);
        _chatroomRepository.Save();

        var retrievedChatroom = _chatroomRepository.GetChatroomById(20);

        Assert.That(retrievedChatroom, Is.Not.Null);
        Assert.That(retrievedChatroom.Name, Is.EqualTo("NewChatroom"));
    }

    [Test]
    public void InsertChatroom_DuplicateId_ThrowsArgumentException()
    {
        var existingChatroom = _chatroomRepository.GetChatroomById(1);

        Assert.Throws<ArgumentException>(() => _chatroomRepository.InsertChatroom(existingChatroom));
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _chatroomRepository.Dispose();
        _context.Dispose();
    }
}