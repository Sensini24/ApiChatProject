using Chat_Project.Models;

namespace Chat_Project.DTOs.ChatParticipantsDTO
{
    public class ChatParticipantsGetDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }
    }
}
