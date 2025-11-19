using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace MovieCatalog.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieCatalogContext _context;

        private readonly IConfiguration _configuration;

        public MoviesController(MovieCatalogContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /*public IActionResult Index()
        {
            var movies = _context.Movies.ToList();
            return View(movies);
        }*/

        private void SetUserViewBag()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }, out var validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var username = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
                    ViewBag.User = username.ToUpper(); // Siempre en mayúsculas
                    ViewBag.Message = $"Welcome, {username.ToUpper()}!";
                }
                catch
                {
                    ViewBag.User = null;
                    ViewBag.Message = "Invalid or expired token.";
                }
            }
            else
            {
                ViewBag.User = null;
                ViewBag.Message = "Unauthorized — Please login.";
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            SetUserViewBag();
            if (ViewBag.User == null) return RedirectToAction("Index", "Login");

            var movies = _context.Movies.ToList();
            return View(movies);
        }

        [HttpGet]
        public IActionResult Create()
        {
            SetUserViewBag();
            if (ViewBag.User == null) return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            SetUserViewBag();
            if (ViewBag.User == null) return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                _context.Movies.Add(movie);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            SetUserViewBag();
            if (ViewBag.User == null) return RedirectToAction("Index", "Login");

            var movie = _context.Movies.Find(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost]
        public IActionResult Edit(Movie updatedMovie)
        {
            SetUserViewBag();
            if (ViewBag.User == null) return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                _context.Movies.Update(updatedMovie);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(updatedMovie);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            SetUserViewBag();
            if (ViewBag.User == null) return RedirectToAction("Index", "Login");

            var movie = _context.Movies.Find(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            SetUserViewBag();
            if (ViewBag.User == null) return RedirectToAction("Index", "Login");

            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();  
            } 
            return RedirectToAction("Index");
            
        }
        
    }
}
