namespace ChatShuttleX.Services.Hubs;

public interface IChatClient
{
    Task ReceiveMessage(string senderUsername, string message, string timestamp);
    Task SendMessage(string message);
}