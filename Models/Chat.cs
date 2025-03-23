namespace Chat_Project.Models;

public class Chat
{
    public int Id { get; set; }
    public string NameChat { get; set; }
    
    public ICollection<Message> Messages { get; set; }
    public ICollection<ChatParticipant> ChatParticipants { get; set; }
}