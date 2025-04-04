using Chat_Project.Models;

namespace Chat_Project.DTOs.MessageDTO
{
    public class MessageGetDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }
    }
}
