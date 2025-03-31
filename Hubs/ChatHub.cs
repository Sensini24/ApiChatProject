using System.Security.Claims;
using Chat_Project.Models;
using Chat_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
namespace Chat_Project.Hubs;

[Authorize]
public class ChatHub:Hub
{
    private readonly UserService _userService;

    
    public ChatHub(UserService userService)
    {
        _userService = userService;
    }


    private static  Dictionary<int,List<string>> connectionsId = new();
    private readonly List<string>? listaInfoUsersConnected = new ();
    
    public override async Task OnConnectedAsync()
    {
        // SortedDictionary<int, int> connectionsId = new SortedDictionary<int, int>();
       
        var usuario = await _userService.ObtenerInfoUser();
        int userId = usuario.UserId;
        string username = usuario.Username;
        string connectionId = Context.ConnectionId;
        
        listaInfoUsersConnected.Add(connectionId);
        listaInfoUsersConnected.Add(username);
        if (connectionsId.ContainsKey(userId))
        {
            connectionsId[userId] = listaInfoUsersConnected;
            await Clients.All.SendAsync("UserConnected", connectionsId);
            await base.OnConnectedAsync();
        }
        else
        {
            connectionsId.Add(userId, listaInfoUsersConnected);
            await Clients.All.SendAsync("UserConnected", connectionsId);
            await base.OnConnectedAsync();
        }
        
    }
    
    
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value); 
        if (connectionsId.ContainsKey(userId))
        {
            //copio la informacion del usuario antes de de ser eliminado paara pasarselo al evento.
            var userIdCopy = connectionsId[userId];
            connectionsId.Remove(userId);
            await Clients.All.SendAsync("UserDisconnected", userIdCopy);
        }
        await base.OnDisconnectedAsync(exception);
    }
    public async Task SendMessageToAll(string message)
    {
        var user = _userService.ObtenerInfoUser();
        string username = user.Result.Username;
        
        var userId =  user.Result.UserId;
        var userIdClaims = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);  
        var connectionId = Context.ConnectionId;
        await Clients.All.SendAsync("ReceiveMessage", username, message, userId, connectionId);
    }

    public async Task isTyping(bool isTyping)
    {
        if (isTyping == true)
        {

        }
    }
}