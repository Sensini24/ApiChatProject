namespace Chat_Project.DTOs.GroupParticipantsDTO
{
    public class GroupParticipantsJoinAddDTO
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string InvitationStatus {  get; set; }
    }
}
