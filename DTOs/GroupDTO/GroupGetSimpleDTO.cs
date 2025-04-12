using Chat_Project.DTOs.GroupParticipantsDTO;

namespace Chat_Project.DTOs.GroupDTO
{
    public class GroupGetSimpleDTO
    {
        public int GroupId { get; set; }
        public string NameGroup { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public string GroupCategory { get; set; }

        public ICollection<GroupParticipantsGetDTO> GroupParticipants { get; set; }
    }
}
