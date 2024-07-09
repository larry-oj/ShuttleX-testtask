using System.Collections.Concurrent;
using System.Globalization;
using ChatShuttleX.Services.Exceptions;
using Microsoft.AspNetCore.SignalR;

namespace ChatShuttleX.Services.Hubs;

public class ChatHub : Hub<IChatClient>
{
    private readonly IUserService _userService;
    private readonly IChatroomService _chatroomService;

    public ChatHub(IUserService userService, IChatroomService chatroomService)
    {
        _userService = userService;
        _chatroomService = chatroomService;
    }

    public async Task JoinChat(string username, int chatId)
    {
        try
        {
            var user = _userService.GetUser(username);
            var chatroom = _chatroomService.GetChatroom(chatId);
            await Groups.AddToGroupAsync(Context.ConnectionId, chatroom.Name);
        }
        catch (Exception e)
        {
            throw e switch
            {
                ChatroomDoesNotExistException or UserDoesNotExistException => new HubException(e.Message),
                _ => new HubException("An error occurred while joining the chat.")
            };
        }
    }
    
    public async Task LeaveChat(string username, int chatId)
    {
        try
        {
            var user = _userService.GetUser(username);
            var chatroom = _chatroomService.GetChatroom(chatId);
            await Groups.AddToGroupAsync(Context.ConnectionId, chatroom.Name);
        }
        catch (Exception e)
        {
            throw e switch
            {
                ChatroomDoesNotExistException or UserDoesNotExistException => new HubException(e.Message),
                _ => new HubException("An error occurred while leaving the chat.")
            };
        }
    }
    
    public async Task SendMessage(string username, int chatId, string message)
    {
        try
        {
            var user = _userService.GetUser(username);
            var chatroom = _chatroomService.GetChatroom(chatId);
            await Clients.Group(chatroom.Name).ReceiveMessage(username, message, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
        }
        catch (Exception e)
        {
            throw e switch
            {
                ChatroomDoesNotExistException or UserDoesNotExistException => new HubException(e.Message),
                _ => new HubException("An error occurred while sending the message.")
            };
        }
    }
}