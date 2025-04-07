using Chat_Project.DTOs.ChatParticipantsDTO;
using Chat_Project.DTOs.MessageDTO;

namespace Chat_Project.DTOs.ChatDTO
{
    public class ChatGetDTO
    {
        public int Id { get; set; }
        public string NameChat { get; set; }

        public ICollection<MessageGetDTO> Messages { get; set; }

        public ICollection<ChatParticipantsGetDTO> ChatParticipants { get; set; }
    }
}
