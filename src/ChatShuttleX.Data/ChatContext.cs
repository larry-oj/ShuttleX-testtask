using ChatShuttleX.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatShuttleX.Data;

public class ChatContext(DbContextOptions<ChatContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Chatroom> Chatrooms { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder); 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // make username unique
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        
        modelBuilder.Entity<Chatroom>()
            .HasIndex(u => u.Name)
            .IsUnique();
    }
}