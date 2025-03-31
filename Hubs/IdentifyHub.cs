using System.Security.Claims;
using Chat_Project.Services;
using Microsoft.AspNetCore.SignalR;

namespace Chat_Project.Hubs;

public class IdentifyHub : Hub
{
    private readonly UserService _userService;

    public IdentifyHub(UserService userService)
    {
        _userService = userService;
    }

    public override async Task OnConnectedAsync()
    {
        var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier);
        var userId = userIdClaim?.Value;
        if (!String.IsNullOrEmpty(userId))
        {
            await Clients.Caller.SendAsync("ReceiveUserId", userId);
        }
        
        await base.OnConnectedAsync();
    }
}