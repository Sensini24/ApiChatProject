using Chat_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat_Project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly DataContext _context;

    public UserController(DataContext context)
    {
        _context = context;
    }
    // GET
    [HttpGet]
    [Route("getUsers")]
    public IActionResult Get()
    {
        var users = _context.Users.ToList();
        return Ok(users);
    }
}