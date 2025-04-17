using System.Security.Claims;
using Chat_Project.Models;
using Chat_Project.Services;
using Microsoft.AspNetCore.SignalR;

namespace Chat_Project.Hubs
{
    public class GroupHub : Hub
    {
        private readonly UserService _userService;


        public GroupHub(UserService userService)
        {
            _userService = userService;
        }

        public async Task JoinChatGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessageToGroup(string groupName, int groupId, string user, string message)
        {
            int userId = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", groupName, groupId, user, userId, message);
        }
    }
}
