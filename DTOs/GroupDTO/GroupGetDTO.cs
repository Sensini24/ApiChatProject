using Chat_Project.DTOs.GroupParticipantsDTO;
using Chat_Project.DTOs.MessageGroupDTO;
using Chat_Project.Models;

namespace Chat_Project.DTOs.GroupDTO
{
    public class GroupGetDTO
    {
        public int GroupId { get; set; }
        public string NameGroup { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public string GroupCategory { get; set; }

        public ICollection<GroupParticipantsGetDTO> GroupParticipants { get; set; }
        public ICollection<MessageGroupGetDTO> GroupMessages { get; set; }
    }
}
