using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Models;
using MovieCatalog.Services;

public class LoginController : Controller
{
    private readonly UserService _userService;
    private readonly JwtService _jwtService;

    private readonly ILogger<LoginController> _logger;

    public LoginController(UserService userService, JwtService jwtService, ILogger<LoginController> logger)
    {
        _userService = userService;
        _jwtService = jwtService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        _logger.LogInformation("Login page requested.");
        return View();
    }

    [HttpPost]
    public IActionResult Index(User user)
    {
        _logger.LogInformation("Login attempt for username: {Username}", user.Username);

        var existingUser = _userService.Authenticate(user.Username, user.Password);
        if (existingUser == null)
        {
            _logger.LogWarning("Invalid login attempt for username: {Username}", user.Username);
            ViewBag.Error = "Invalid credentials";
            return View();
        }

        var token = _jwtService.GenerateToken(existingUser);

        _logger.LogInformation("User {Username} successfully authenticated. JWT generated.", user.Username);

        HttpContext.Session.SetString("JWToken", token);
        HttpContext.Session.SetString("CurrentUsername", existingUser.Username);
        return RedirectToAction("Index", "Movies");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        _logger.LogInformation("User logged out.");
        
        HttpContext.Session.Remove("JWToken");
        return RedirectToAction("Index", "Login");
    }

}