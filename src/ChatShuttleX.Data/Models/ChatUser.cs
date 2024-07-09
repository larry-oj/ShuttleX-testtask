using System.ComponentModel.DataAnnotations;

namespace ChatShuttleX.Data.Models;

public class ChatUser : Entity
{
    [Required]
    public Chatroom Chatroom { get; set; }
    
    [Required]
    public User User { get; set; }
}