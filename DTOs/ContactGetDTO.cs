using Chat_Project.Models;

namespace Chat_Project.DTOs
{
    public class ContactGetDTO
    {
        public int ContactId { get; set; }
        public int UserId { get; set; }
        public int ContactUserId { get; set; }
        public string? NickName { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }
    }
}
