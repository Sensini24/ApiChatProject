using Chat_Project.Models;

namespace Chat_Project.DTOs.MessageDTO
{
    public class MessageAddDTO
    {
        public int UserId { get; set; }
        public string MessageText { get; set; }
    }
}
