using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Models;
using MovieCatalog.Services;

public class RegisterController : Controller
{
    private readonly UserService _userService;

    public RegisterController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Index(User user)
    {
        var success = _userService.Register(user);
        if (success)
            return RedirectToAction("Index", "Login");

        ViewBag.Error = "Username already exists";
        return View();
    }
}