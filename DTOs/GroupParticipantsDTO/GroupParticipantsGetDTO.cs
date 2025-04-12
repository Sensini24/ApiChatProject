using Chat_Project.Models;

namespace Chat_Project.DTOs.GroupParticipantsDTO
{
    public class GroupParticipantsGetDTO
    {
        public int GroupParticipantsId { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public DateTime? DateJoined { get; set; }
        public string InvitationStatus { get; set; }
        public string Rol { get; set; }
        public bool isFavorite { get; set; }
    }
}
