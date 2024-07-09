using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatShuttleX.Data.Models;

public class User : Entity
{
    [Required]
    public string Username { get; set; }
}