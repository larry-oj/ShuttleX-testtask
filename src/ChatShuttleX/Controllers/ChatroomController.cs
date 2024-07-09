using ChatShuttleX.Services;
using ChatShuttleX.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatShuttleX.Controllers;

[ApiController]
[Route("chats")]
public class ChatroomController(IChatroomService chatroomService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            var chatrooms = chatroomService.GetChatrooms();
            return Ok(chatrooms);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    [Route("search")]
    public IActionResult SearchChatrooms([FromBody] ChatroomSearch search)
    {
        try
        {
            var chatrooms = chatroomService.GetChatrooms(search.Query.ToLower());
            return Ok(chatrooms);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    [Route("create")]
    public IActionResult CreateChatroom([FromBody] ChatroomModel chatroom)
    {
        try
        {
            if (string.IsNullOrEmpty(chatroom.Name))
            {
                return BadRequest("Chatroom name is invalid");
            }
            
            chatroomService.CreateChatroom(chatroom.Name, chatroom.Owner.Username);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete]
    [Route("delete")]
    public IActionResult DeleteChatroom([FromBody] ChatroomDelete obj)
    {
        try
        {
            var chatroom = chatroomService.GetChatroom(obj.ChatroomId);
            if (chatroom.Owner.Username != obj.Username)
            {
                return BadRequest("You are not the owner of this chatroom");
            }
            chatroomService.DeleteChatroom(obj.ChatroomId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}