using Chat_Project.Models;

namespace Chat_Project.DTOs.MessageGroupDTO
{
    public class MessageGroupGetDTO
    {
        public int MessagesGroupId { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageDate { get; set; }
    }
}
