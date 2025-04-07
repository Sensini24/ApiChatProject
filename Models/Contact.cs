namespace Chat_Project.Models
{
    public class Contact
    {
        public int ContactId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ContactUserId { get; set; }
        public User ContactUser { get; set; }
        public string? NickName { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsFavorite{ get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBlocked { get; set; }
    }
}
