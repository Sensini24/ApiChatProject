using Chat_Project.Models;

namespace Chat_Project.DTOs.MessageGroupDTO
{
    public class MessageGroupAddDTO
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string MessageText { get; set; }
    }
}
