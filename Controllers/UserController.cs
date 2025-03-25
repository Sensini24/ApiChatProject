using Microsoft.AspNetCore.Mvc;

namespace Chat_Project.Controllers;

public class UserController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}