namespace Chat_Project.Models;

public class Message
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int ChatId { get; set; }
    public Chat Chat { get; set; }
    public string MessageText { get; set; }
    public DateTime MessageDate { get; set; }
    public bool IsRead { get; set; }
    public bool IsDeleted { get; set; }
}