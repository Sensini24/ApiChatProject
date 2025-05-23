using System.Security.Claims;
using Chat_Project.Data;
using Chat_Project.DTOs.UserDTO;
using Chat_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat_Project.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly DataContext _context;

    public UserController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("getCurrentUser")]
    public async Task<IActionResult> GetUserCurrent()
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _context.Users.FromSqlInterpolated($"Select * From Users WHERE UserId = {userId}").FirstAsync();

            var userdto = new UserInfoDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Gender = (Gender)user.Gender// Convertir el valor del enum a string
            };
            return Ok(new
            {
                success = true,
                userdto
            });
        }
        catch (Exception e)
        {
            return BadRequest(new
            {
                success = false,
                e
            });
        }
    }

    [HttpGet]
    [Route("getUser/{userId}")]
    public async Task<IActionResult> GetUser([FromRoute] int userId)
    {
        try
        {
            var user = await _context.Users.FromSqlInterpolated($"Select * From Users WHERE UserId = {userId}").FirstAsync();

            var userdto = new UserInfoDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Gender =  (Gender)user.Gender// Convertir el valor del enum a string
            };
            return Ok(new
            {
                success = true,
                userdto
            });
        }
        catch (Exception e)
        {
            return BadRequest(new
            {
                success = false,
                e
            });
        }
    }


    [HttpGet]
    [Route("getUsers")]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await _context.Users.FromSqlInterpolated($"Select * From Users").ToListAsync();

            var userdto = users.Select(x => new UserInfoDTO
            {
                UserId = x.UserId,
                Username = x.Username,
                Email = x.Email,
                Gender = (Gender)x.Gender// Convertir el valor del enum a string
            });


            return Ok(userdto);
        }
        catch (Exception e)
        {
            return BadRequest(new
            {
                success = false,
                e
            });
        }
    }

    [HttpGet]
    [Route("findUser/{initials}")]
    public async Task<IActionResult> FindUsersByInitials(string initials)
    {
        try
        {
            var users = await _context.Users.FromSqlInterpolated($"SELECT * FROM Users where Username LIKE {initials+"%"}").ToListAsync();
            //var userFound = await _context.Users.Where(x => x.Username.Contains(initials)).ToListAsync();
            var userdto = users.Select(x => new UserInfoDTO
            {
                UserId = x.UserId,
                Username = x.Username,
                Email = x.Email,
                Gender = (Gender)x.Gender// Convertir el valor del enum a string
            });


            return Ok(new
            {
                userdto
            });
        }
        catch (Exception e)
        {
            return BadRequest(new
            {
                success = false,
                e
            });
        }
    }

    
}