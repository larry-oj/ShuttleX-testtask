using ChatShuttleX.Data;
using ChatShuttleX.Data.Models;
using ChatShuttleX.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatShuttleX.Tests.Repositories;

public class ChatUserRepoTests
{
    private IChatroomRepository _chatroomRepository;
    private IChatUserRepository _chatuserRepository;
    private ChatContext _context;
    
    [OneTimeSetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ChatContext>()
            .UseInMemoryDatabase(databaseName: "chatshuttlex_test")
            .Options;
        
        _context = new ChatContext(options);
        _chatroomRepository = new ChatroomRepository(_context);
        _chatuserRepository = new ChatUserRepository(_context);

        // Seed data
        var user1 = new User { Id = 1, Username = "user1" };
        var user2 = new User { Id = 2, Username = "user2" };
        var chatroom1 = new Chatroom { Id = 1, Name = "Chatroom1", Creator = user1 };
        var chatroom2 = new Chatroom { Id = 2, Name = "Chatroom2", Creator = user2 };
        
        _context.AddRange(user1, user2, chatroom1, chatroom2);
        
        var chatusers = new List<ChatUser>
        {
            new ChatUser { Id = 1, User = user1, Chatroom = chatroom1 },
            new ChatUser { Id = 2, User = user2, Chatroom = chatroom2 },
        };
        
        _context.AddRange(chatusers);
        _context.SaveChanges();
    }

    [Test]
    public void GetChatUsers_ReturnsAllChatUsers()
    {
        var chatUsers = _chatuserRepository.GetChatUsers();

        Assert.That(chatUsers.Count(), Is.EqualTo(2));
    }

    [Test]
    public void GetChatUsersByChatroomId_ValidId_ReturnsChatUsers()
    {
        var chatUsers = _chatuserRepository.GetChatUsersByChatroomId(1);

        Assert.That(chatUsers.Count(), Is.EqualTo(1));
        Assert.That(chatUsers.First().User.Username, Is.EqualTo("user1"));
    }

    [Test]
    public void GetChatUsersByChatroomId_InvalidId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _chatuserRepository.GetChatUsersByChatroomId(999));
    }

    [Test]
    public void InsertChatUser_ValidChatUser_InsertsSuccessfully()
    {
        var newUser = new User { Id = 3, Username = "user3" };
        var newChatroom = new Chatroom { Id = 3, Name = "Chatroom3", Creator = newUser };
        var newChatUser = new ChatUser { Id = 3, User = newUser, Chatroom = newChatroom };

        _chatuserRepository.InsertChatUser(newChatUser);
        _chatuserRepository.Save();

        var retrievedChatUser = _chatuserRepository.GetChatUsersByChatroomId(3).FirstOrDefault();

        Assert.That(retrievedChatUser, Is.Not.Null);
        Assert.That(retrievedChatUser.User.Username, Is.EqualTo(newUser.Username));
        Assert.That(retrievedChatUser.Chatroom.Name, Is.EqualTo(newChatroom.Name));
    }

    [Test]
    public void InsertChatUser_DuplicateChatUser_ThrowsArgumentException()
    {
        var existingChatUser = _chatuserRepository.GetChatUsers().First();

        Assert.Throws<ArgumentException>(() => _chatuserRepository.InsertChatUser(existingChatUser));
    }

    [Test]
    public void DeleteChatUser_ValidId_DeletesSuccessfully()
    {
        var chatUserToDelete = _chatuserRepository.GetChatUsers().First();

        _chatuserRepository.DeleteChatUser(chatUserToDelete.Id);
        _chatuserRepository.Save();

        var deletedChatUser = _chatuserRepository.GetChatUsersByChatroomId(chatUserToDelete.Chatroom.Id)
                                                  .FirstOrDefault(cu => cu.Id == chatUserToDelete.Id);

        Assert.That(deletedChatUser, Is.Null);
    }

    [Test]
    public void DeleteChatUser_InvalidId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _chatuserRepository.DeleteChatUser(999));
    }
    
    [Test]
    public void GetChatUsersByChatroom_ValidChatroom_ReturnsChatUsers()
    {
        var chatroom = _chatroomRepository.GetChatroomById(1);
        var chatUsers = _chatuserRepository.GetChatUsersByChatroom(chatroom);

        Assert.That(chatUsers.Count(), Is.EqualTo(1));
        Assert.That(chatUsers.First().User.Username, Is.EqualTo("user1"));
    }

    [Test]
    public void GetChatUsersByChatroom_InvalidChatroom_ThrowsArgumentException()
    {
        var invalidChatroom = new Chatroom { Id = 999, Name = "InvalidChatroom", Creator = new User { Id = 999, Username = "invaliduser" } };

        Assert.Throws<ArgumentException>(() => _chatuserRepository.GetChatUsersByChatroom(invalidChatroom));
    }

    [Test]
    public void UpdateChatUser_ValidChatUser_UpdatesSuccessfully()
    {
        var chatUserToUpdate = _chatuserRepository.GetChatUsers().First();
        var originalUsername = chatUserToUpdate.User.Username;
        var newUsername = "updatedUser";

        chatUserToUpdate.User.Username = newUsername;
        _chatuserRepository.UpdateChatUser(chatUserToUpdate);
        _chatuserRepository.Save();

        var updatedChatUser = _chatuserRepository.GetChatUsersByChatroomId(chatUserToUpdate.Chatroom.Id)
            .FirstOrDefault(cu => cu.Id == chatUserToUpdate.Id);

        Assert.That(updatedChatUser, Is.Not.Null);
        Assert.That(updatedChatUser.User.Username, Is.EqualTo(newUsername));
    }

    [Test]
    public void UpdateChatUser_InvalidChatUser_ThrowsArgumentException()
    {
        var invalidChatUser = new ChatUser { Id = 999, User = new User { Id = 999, Username = "invalidUser" }, Chatroom = new Chatroom { Id = 999, Name = "InvalidChatroom", Creator = new User { Id = 999, Username = "invalidCreator" } } };

        Assert.Throws<ArgumentException>(() => _chatuserRepository.UpdateChatUser(invalidChatUser));
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _chatroomRepository.Dispose();
        _chatuserRepository.Dispose();
        _context.Dispose();
    }
}