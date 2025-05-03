using Chat_Project.Models;

namespace Chat_Project.DTOs.FilePrivateChatDTO
{
    public class FileUploadGetDTO
    {
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public DateTime UploadDate { get; set; }
        public string? FileType { get; set; }
        public string? FileExtension { get; set; }
        public string FilePath { get; set; }

    }
}
