using ChatShuttleX.Controllers;
using ChatShuttleX.Services;
using ChatShuttleX.Services.Hubs;
using ChatShuttleX.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ChatShuttleX.IntegrationTests;

public class ChatroomControllerTests
{
    private Mock<IChatroomService> _mockChatroomService;
    private Mock<ChatHub> _mockChatHub;
    private ChatroomController _controller;

    [SetUp]
    public void Setup()
    {
        _mockChatroomService = new Mock<IChatroomService>();
        _mockChatHub = new Mock<ChatHub>(_mockChatroomService.Object);
        _controller = new ChatroomController(_mockChatroomService.Object, _mockChatHub.Object);
    }

    [Test]
    public void GetAll_ReturnsOkWithChatrooms()
    {
        var chatrooms = new List<ChatroomModel> { new ChatroomModel { Name = "testroom" } };
        _mockChatroomService.Setup(service => service.GetChatrooms("")).Returns(chatrooms);

        var result = _controller.GetAll();

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnValue = okResult.Value as List<ChatroomModel>;
        Assert.IsNotNull(returnValue);
        Assert.AreEqual(1, returnValue.Count);
    }

    [Test]
    public void SearchChatrooms_WithValidQuery_ReturnsOkWithChatrooms()
    {
        var chatrooms = new List<ChatroomModel> { new ChatroomModel { Name = "testroom" } };
        _mockChatroomService.Setup(service => service.GetChatrooms(It.IsAny<string>())).Returns(chatrooms);
        var searchQuery = new ChatroomSearch { Query = "test" };

        var result = _controller.SearchChatrooms(searchQuery);

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnValue = okResult.Value as List<ChatroomModel>;
        Assert.IsNotNull(returnValue);
        Assert.AreEqual(1, returnValue.Count);
    }

    [Test]
    public void CreateChatroom_WithValidChatroom_ReturnsOk()
    {
        var chatroom = new ChatroomModel { Name = "testroom", Owner = new UserModel { Username = "testuser" } };

        var result = _controller.CreateChatroom(chatroom);

        Assert.IsInstanceOf<OkResult>(result);
    }

    [Test]
    public void CreateChatroom_WithEmptyName_ReturnsBadRequest()
    {
        var chatroom = new ChatroomModel { Name = "", Owner = new UserModel { Username = "testuser" } };

        var result = _controller.CreateChatroom(chatroom);

        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual("Chatroom name is invalid", badRequestResult.Value);
    }
    
    [Test]
    public void DeleteChatroom_WithValidRequest_ReturnsOk()
    {
        var chatroom = new ChatroomModel { Id = 1, Owner = new UserModel { Username = "testuser" } };
        _mockChatroomService.Setup(service => service.GetChatroom(It.IsAny<int>())).Returns(chatroom);
        var deleteRequest = new ChatroomDelete { ChatroomId = 1, Username = "testuser" };

        var result = _controller.DeleteChatroom(deleteRequest);

        Assert.IsInstanceOf<OkResult>(result);
        _mockChatroomService.Verify(service => service.DeleteChatroom(1), Times.Once);
    }

    [Test]
    public void DeleteChatroom_WithNonOwnerRequest_ReturnsBadRequest()
    {
        var chatroom = new ChatroomModel { Id = 1, Owner = new UserModel { Username = "anotheruser" } };
        _mockChatroomService.Setup(service => service.GetChatroom(It.IsAny<int>())).Returns(chatroom);
        var deleteRequest = new ChatroomDelete { ChatroomId = 1, Username = "testuser" };

        var result = _controller.DeleteChatroom(deleteRequest).Result;

        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual("You are not the owner of this chatroom", badRequestResult.Value);
    }

    [Test]
    public void DeleteChatroom_WithNonExistingChatroom_ReturnsBadRequest()
    {
        _mockChatroomService.Setup(service => service.GetChatroom(It.IsAny<int>())).Throws(new Exception("Chatroom not found"));
        var deleteRequest = new ChatroomDelete { ChatroomId = 1, Username = "testuser" };

        var result = _controller.DeleteChatroom(deleteRequest).Result;

        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual("Chatroom not found", badRequestResult.Value);
    }
}