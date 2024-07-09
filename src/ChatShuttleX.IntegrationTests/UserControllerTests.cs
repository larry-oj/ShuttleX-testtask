using ChatShuttleX.Controllers;
using ChatShuttleX.Services;
using ChatShuttleX.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ChatShuttleX.IntegrationTests;

public class UserControllerTests
{
    private Mock<IUserService> _mockUserService;
    private UserController _controller;

    [SetUp]
    public void Setup()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UserController(_mockUserService.Object);
    }

    [Test]
    public void RegisterUser_WithValidUser_ReturnsOk()
    {
        var user = new UserModel { Username = "testuser" };
        _mockUserService.Setup(service => service.UserExists(It.IsAny<string>())).Returns(false);
        
        var result = _controller.RegisterUser(user);
        
        Assert.IsInstanceOf<OkResult>(result);
    }

    [Test]
    public void RegisterUser_WithExistingUser_ReturnsBadRequest()
    {
        var user = new UserModel { Username = "testuser" };
        _mockUserService.Setup(service => service.UserExists(It.IsAny<string>())).Returns(true);
        
        var result = _controller.RegisterUser(user);
        
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual("User already exists", badRequestResult.Value);
    }

    [Test]
    public void RegisterUser_WithEmptyUsername_ReturnsBadRequest()
    {
        var user = new UserModel { Username = "" };
        
        var result = _controller.RegisterUser(user);
        
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual("Username is invalid", badRequestResult.Value);
    }

    [Test]
    public void GetUser_WithExistingUser_ReturnsOk()
    {
        var user = new UserModel { Username = "testuser" };
        _mockUserService.Setup(service => service.GetUser(It.IsAny<string>())).Returns(user);
        
        var result = _controller.GetUser("testuser");
        
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(user, okResult.Value);
    }

    [Test]
    public void GetUser_WithNonExistingUser_ReturnsBadRequest()
    {
        _mockUserService.Setup(service => service.GetUser(It.IsAny<string>())).Throws(new Exception("User not found"));
        
        var result = _controller.GetUser("nonexistinguser");
        
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual("User not found", badRequestResult.Value);
    }

    [Test]
    public void DeleteUser_WithExistingUser_ReturnsOk()
    {
        _mockUserService.Setup(service => service.DeleteUser(It.IsAny<string>())).Verifiable();
        
        var result = _controller.DeleteUser("testuser");
        
        Assert.IsInstanceOf<OkResult>(result);
        _mockUserService.Verify(service => service.DeleteUser("testuser"), Times.Once);
    }

    [Test]
    public void DeleteUser_WithNonExistingUser_ReturnsBadRequest()
    {
        _mockUserService.Setup(service => service.DeleteUser(It.IsAny<string>())).Throws(new Exception("User not found"));
        
        var result = _controller.DeleteUser("nonexistinguser");
        
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual("User not found", badRequestResult.Value);
    }
}