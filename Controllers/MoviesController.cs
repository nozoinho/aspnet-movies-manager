using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using MongoDB.Bson.IO;

namespace MovieCatalog.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieService _movieService;

        private readonly IConfiguration _configuration;

        public MoviesController(MovieService movieService, IConfiguration configuration)
        {
            _movieService = movieService;
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
                    var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                    var username = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
                    ViewBag.User = username.ToUpper();
                    ViewBag.UserId = userId;
                    ViewBag.Message = $"Welcome, {username.ToUpper()}!";
                }
                catch
                {
                    ViewBag.User = null;
                    ViewBag.UserId = null;
                    ViewBag.Message = "Invalid or expired token.";
                }
            }
            else
            {
                ViewBag.User = null;
                ViewBag.UserId = null;
                ViewBag.Message = "Unauthorized — Please login.";
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            SetUserViewBag();
            if (ViewBag.User == null) return RedirectToAction("Index", "Login");

            var movies = _movieService.GetByUser(ViewBag.UserId);
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
                movie.UserId = ViewBag.UserId;
                _movieService.Create(movie);
                return RedirectToAction("Index", "Movies");
            }
            return View(movie);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            SetUserViewBag();
            var userId = ViewBag.UserId;
            if (ViewBag.User == null) return RedirectToAction("Index", "Login");

            var movie = _movieService.Get(id);

            if (movie == null || movie.UserId != userId)
                return Unauthorized();

            return View(movie);
        }

        [HttpPost]
        public IActionResult Edit(Movie updatedMovie)
        {
            SetUserViewBag();
            var userId = ViewBag.UserId;

            if (ViewBag.User == null) return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(updatedMovie.Id))
                {
                    ModelState.AddModelError("", "Movie ID is missing.");
                    return View(updatedMovie);
                }

                var movieInDb = _movieService.Get(updatedMovie.Id);

                if (movieInDb == null || movieInDb.UserId != userId)
                    return Unauthorized();

                updatedMovie.UserId = movieInDb.UserId;
                
                _movieService.Update(updatedMovie.Id!, updatedMovie);
                return RedirectToAction("Index", "Movies");
            }
            return View(updatedMovie);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            SetUserViewBag();
            var userId = ViewBag.UserId;

            if (ViewBag.User == null) return RedirectToAction("Index", "Login");

            var movie = _movieService.Get(id);

            if (movie == null || movie.UserId != userId)
                    return Unauthorized();

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            SetUserViewBag();
            var userId = ViewBag.UserId;

            if (ViewBag.User == null) return RedirectToAction("Index", "Login");

            var movie = _movieService.Get(id);

            if (movie == null || movie.UserId != userId)
                return Unauthorized();
            
            _movieService.Remove(id);

            return RedirectToAction("Index", "Movies");
            
        }
        
    }
}
