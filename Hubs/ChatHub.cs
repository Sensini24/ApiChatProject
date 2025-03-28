using Chat_Project.Models;
using Chat_Project.Services;
using Microsoft.AspNetCore.SignalR;

namespace Chat_Project.Hubs;

public class ChatHub:Hub
{
    private readonly UserService _userService;

    public ChatHub(UserService userService)
    {
        _userService = userService;
    }
    
    public async Task SendMessageToAll(string message)
    {
        string username = _userService.ObtenerInfoUser().Result.Username;
        await Clients.All.SendAsync("ReceiveMessage", username, message);
    }
}