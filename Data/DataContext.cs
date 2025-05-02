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
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupParticipants> GroupParticipants { get; set; }
    public DbSet<MessagesGroup> MessagesGroups { get; set; }
    public DbSet<FilePrivateChat> FilePrivateChats { get; set; }

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

        modelBuilder.Entity<Contact>()
            .HasOne(p=>p.User)
            .WithMany(c=>c.Contacts) //UN usuario puede tener muchos contacto, pero un contacto en particular es de un usuario.
            .HasForeignKey(p=>p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Contact>()
            .HasOne(p => p.ContactUser)
            .WithMany()
            .HasForeignKey(p => p.ContactUserId)
            .OnDelete(DeleteBehavior.Restrict);
          
        modelBuilder.Entity<GroupParticipants>()
            .HasOne(p=>p.User)
            .WithMany(p=>p.GroupParticipants)
            .HasForeignKey(p=>p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GroupParticipants>()
            .HasOne(p => p.Group)
            .WithMany(p => p.GroupParticipants)
            .HasForeignKey(p => p.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MessagesGroup>()
            .HasOne(p => p.User)
            .WithMany(p => p.GroupMessages)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MessagesGroup>()
            .HasOne(p => p.Group)
            .WithMany(p => p.GroupMessages)
            .HasForeignKey(p => p.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FilePrivateChat>()
            .HasOne(p=>p.User)
            .WithMany(p=>p.FilePrivateChats)
            .HasForeignKey(p=>p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FilePrivateChat>()
            .HasOne(p => p.Chat)
            .WithMany(p => p.FilePrivateChats)
            .HasForeignKey(p=>p.ChatId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FilePrivateChat>()
                 .Property(o => o.FileSize)
                 .HasColumnType("decimal(18, 2)");
    }
}