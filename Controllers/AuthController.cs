using System.Security.Claims;
using Chat_Project.Data;
using Chat_Project.DTOs.AuthDTO;
using Chat_Project.Models;
using Chat_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat_Project.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController:Controller
{
    private readonly DataContext _db;
    private readonly ITokenService _tokenService;
    
    public AuthController(DataContext db, ITokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO logindto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isUser = await _db.Users.AnyAsync(e=>e.Email == logindto.Email && e.PasswordHash == logindto.Password);
        if (isUser)
        {
            var user = await _db.Users.Where(u=>u.Email== logindto.Email).FirstOrDefaultAsync();
            var token = _tokenService.CrearToken(user.UserId);
            SetAuthCookie(token);
            return Ok(new { success = true, message = "Usuario aceptado", Token = token});
        }
        return Unauthorized(new
        {
            success = false,
            message= "Crendenciales incorrectas",
            data = logindto
        });
    }
    
    private void SetAuthCookie(string token)
    {
        Response.Cookies.Append("tokenUser", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(7)
        });
    }
    private IActionResult  addTokenToCookie(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return NotFound(new
            {
                success = false,
                message = "No se obtuvo el token",
            });
        }

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("tokenUser", token, cookieOptions);
            
        return Ok(new
        {
            success = true,
            message = "Token guardado en cookies"
        });
    }
    
    
    [HttpPost]
    [Route("registerUser")]
    public async Task<IActionResult> Add([FromBody] RegisterDTO userdto)
    {
        if (userdto == null)
        {
            return BadRequest(new { success = false, message = "Los datos del usuario no son correctos" });
        }
        var existEmail = _db.Users.Where(e => e.Email == userdto.Email);
        if (existEmail.Any())
        {
            return Conflict(new { message = "El email ya está en uso" });
        }

        //Registrar el usuario
        var newUser = new User()
        {
            Username = userdto.Username,
            Email = userdto.Email,
            PasswordHash = userdto.PasswordHash,
            Gender = userdto.Gender,
        };

        await _db.Users.AddAsync(newUser);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            Success = true,
            newUser
        });
    }
}