using Chat_Project.Models;

namespace Chat_Project.DTOs.FilePrivateChatDTO
{
    public class FileUploadGetDTO
    {
        public string NameChat { get; set; }
        public IFormFile file { get; set; }

    }
}
