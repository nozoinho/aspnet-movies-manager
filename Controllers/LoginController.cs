using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Models;
using MovieCatalog.Services;

public class LoginController : Controller
{
    private readonly UserService _userService;
    private readonly JwtService _jwtService;

    public LoginController(UserService userService, JwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Index(User user)
    {
        var existingUser = _userService.Authenticate(user.Username, user.Password);
        if (existingUser == null)
        {
            ViewBag.Error = "Invalid credentials";
            return View();
        }

        var token = _jwtService.GenerateToken(existingUser.Username);
        HttpContext.Session.SetString("JWToken", token);
        return RedirectToAction("Index", "Movies");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("JWToken");
        return RedirectToAction("Index", "Login");
    }

}