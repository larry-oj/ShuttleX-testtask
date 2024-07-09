using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatShuttleX.Data.Models;

public class Chatroom : Entity
{
    [Required]
    public User Creator { get; set; }
    
    [Required]
    public string Name { get; set; }
}