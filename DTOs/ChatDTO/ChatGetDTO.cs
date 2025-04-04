using Chat_Project.DTOs.ChatParticipantsDTO;
using Chat_Project.DTOs.MessageDTO;

namespace Chat_Project.DTOs.ChatDTO
{
    public class ChatGetDTO
    {
        public int Id { get; set; }
        public string NameChat { get; set; }

        public ICollection<MessageAddDTO> Messages { get; set; }

        public ICollection<ChatParticipantsAddDTO> ChatParticipants { get; set; }
    }
}
