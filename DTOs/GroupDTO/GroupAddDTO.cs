using Chat_Project.DTOs.GroupParticipantsDTO;
using Chat_Project.Models;

namespace Chat_Project.DTOs.GroupDTO
{
    public class GroupAddDTO
    {
        public string NameGroup { get; set; }
        public string GroupCategory { get; set; }

        public ICollection<GroupParticipantsAddDTO> GroupParticipants { get; set; }
    }
}
