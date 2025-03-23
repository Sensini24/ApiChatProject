using Chat_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat_Project.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {

    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ChatParticipant> ChatParticipants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ChatParticipant>()
            .HasOne(p=>p.Chat)
            .WithMany(p=>p.ChatParticipants)
            .HasForeignKey(p=>p.ChatId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ChatParticipant>()
            .HasOne(p=>p.User)
            .WithMany(p=>p.ChatParticipants)
            .HasForeignKey(p=>p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Message>()
            .HasOne(p => p.Chat)
            .WithMany(p => p.Messages)
            .HasForeignKey(p=>p.ChatId)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        modelBuilder.Entity<Message>()
            .HasOne(p => p.User)
            .WithMany(p => p.Messages)
            .HasForeignKey(p=>p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}