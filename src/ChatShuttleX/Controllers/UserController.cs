using ChatShuttleX.Services;
using ChatShuttleX.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatShuttleX.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost]
    [Route("register")]
    public IActionResult RegisterUser([FromBody] UserModel user)
    {
        try
        {
            if (string.IsNullOrEmpty(user.Username))
            {
                return BadRequest("Username is invalid");
            }
            
            if (!userService.UserExists(user.Username))
            {
                return BadRequest("User already exists");
            }
            
            userService.Register(user.Username);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Route("get/{username}")]
    public IActionResult GetUser([FromRoute] string username)
    {
        try
        {
            var user = userService.GetUser(username);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete]
    [Route("delete/{username}")]
    public IActionResult DeleteUser([FromRoute] string username)
    {
        try
        {
            userService.DeleteUser(username);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}