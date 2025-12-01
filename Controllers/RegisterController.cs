using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Models;
using MovieCatalog.Services;

public class RegisterController : Controller
{
    private readonly UserService _userService;

    private readonly ILogger<RegisterController> _logger;

    public RegisterController(UserService userService, ILogger<RegisterController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        _logger.LogInformation("Registration page requested.");
        return View();
    }

    [HttpPost]
    public IActionResult Index(User user)
    {
        _logger.LogInformation("Registration attempt for username: {Username}", user.Username);

        var success = _userService.Register(user);
        if (success)
        {
            _logger.LogInformation("User {Username} registered successfully.", user.Username);

            return RedirectToAction("Index", "Login");
        }

        _logger.LogWarning("Registration failed. Username already exists: {Username}", user.Username);

        ViewBag.Error = "Username already exists";
        return View();
    }
}