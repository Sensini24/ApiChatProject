namespace Chat_Project.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string NameGroup { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public string GroupCategory { get; set; }

        public ICollection<GroupParticipants> GroupParticipants { get; set; }
        public ICollection<MessagesGroup> GroupMessages { get; set; }

    }
}
