using Chat_Project.DTOs.ChatParticipantsDTO;
using Chat_Project.DTOs.MessageDTO;
using Chat_Project.Models;

namespace Chat_Project.DTOs.ChatDTO
{
    public class ChatAddDTO
    {
        public string NameChat { get; set; }

        public ICollection<MessageAddDTO> Messages { get; set; }

        public ICollection<ChatParticipantsAddDTO> ChatParticipants { get; set; }
    }
}
