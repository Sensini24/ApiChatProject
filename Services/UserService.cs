using System.Security.Claims;
using Chat_Project.Data;
using Chat_Project.Models;
namespace Chat_Project.Services;

public class UserService
{
    private readonly DataContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(DataContext db, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _db = db;
    }
    public async Task<User> ObtenerInfoUser()
    {
        var stringId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        int userId  = int.Parse(stringId);
        var user = await _db.Users.FindAsync(userId);
        return user;
    }
}