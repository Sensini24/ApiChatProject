using Chat_Project.Models;

namespace Chat_Project.DTOs.UserDTO
{
    public class UserInfoDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }

        //// Propiedad de navegación para mensajes enviados
        //public ICollection<Message> Messages { get; set; }
        //// Propiedad de navegación para chats en los que participa
        //public ICollection<ChatParticipant> ChatParticipants { get; set; }
    }
}
