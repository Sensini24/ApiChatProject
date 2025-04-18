namespace Chat_Project.DTOs.GroupDTO
{
    public class GroupSearcherdGetDTO
    {
        public int GroupId { get; set; }
        public string NameGroup { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public string GroupCategory { get; set; }
    }
}
