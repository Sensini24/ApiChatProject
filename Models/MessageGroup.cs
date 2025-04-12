using System.Text.RegularExpressions;

namespace Chat_Project.Models
{
    public class MessagesGroup
    {
        public int MessagesGroupId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }
    }
}
