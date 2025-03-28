using Chat_Project.Models;

namespace Chat_Project.DTOs.AuthDTO;

public class RegisterDTO
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public Gender Gender { get; set; }
}