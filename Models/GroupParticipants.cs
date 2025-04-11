namespace Chat_Project.Models
{
    public class GroupParticipants
    {
        public int GroupParticipantsId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public DateTime? DateJoined { get; set; }
        public string InvitationStatus { get; set; }
        public string Rol { get; set; }
        public bool isFavorite { get; set; }
    }
}
