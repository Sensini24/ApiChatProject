namespace Chat_Project.Models
{
    public class FilePrivateChat
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Chat Chat { get; set; }
        public int ChatId { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public DateTime UploadDate { get; set; }
        public string? FileType { get; set; }
        public string? FileExtension { get; set; }
        public string FilePath { get; set; }
    }
}
  