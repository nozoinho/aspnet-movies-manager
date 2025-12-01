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

        private readonly ILogger<MoviesController> _logger;

        public MoviesController(MovieService movieService, IConfiguration configuration, ILogger<MoviesController> logger)
        {
            _movieService = movieService;
            _configuration = configuration;
            _logger = logger;
        }

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
                    _logger.LogWarning("Invalid or expired token detected during Movie action.");

                    ViewBag.User = null;
                    ViewBag.UserId = null;
                    ViewBag.Message = "Invalid or expired token.";
                }
            }
            else
            {
                _logger.LogWarning("Unauthorized access attempt — no token in session.");

                ViewBag.User = null;
                ViewBag.UserId = null;
                ViewBag.Message = "Unauthorized — Please login.";
            }
        }

        // GET: Movies
        [HttpGet]
        public IActionResult Index()
        {
            SetUserViewBag();
            if (ViewBag.User == null)
            {
                _logger.LogWarning("Unauthorized attempt to access Movies index.");

                return RedirectToAction("Index", "Login");
            }
            
            var userName = (string?)ViewBag.User;
            _logger.LogInformation("User {UserName} accessed Movies index.", userName);

            var movies = _movieService.GetByUser(ViewBag.UserId);
            return View(movies);
        }

        // GET: Create
        [HttpGet]
        public IActionResult Create()
        {
            SetUserViewBag();
            if (ViewBag.User == null)
            {
                _logger.LogWarning("Unauthorized attempt to access Create Movie page.");

                return RedirectToAction("Index", "Login");
            } 

            var userName = (string?)ViewBag.User;
            _logger.LogInformation("User {UserId} opened Create Movie page.", userName);


            return View();
        }

        // POST: Create
        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            SetUserViewBag();
            if (ViewBag.User == null)
            {
                _logger.LogWarning("Unauthorized POST attempt to create a movie.");

                return RedirectToAction("Index", "Login");
            } 

            var userName = (string?)ViewBag.User;

            if (ModelState.IsValid)
            {
                movie.UserId = ViewBag.UserId;
                
                _logger.LogInformation(
                    "User {UserName} created a new movie: {Title}.", userName, movie.Title);

                _movieService.Create(movie);
                return RedirectToAction("Index", "Movies");
            }

            _logger.LogWarning("Invalid model state while creating movie by user {UserName}.", userName);

            return View(movie);
        }

        // GET: Edit
        [HttpGet]
        public IActionResult Edit(string id)
        {
            SetUserViewBag();
            var userId = ViewBag.UserId;
            if (ViewBag.User == null)
            {
                _logger.LogWarning("Unauthorized attempt to access Edit page.");

                return RedirectToAction("Index", "Login");
            } 

            var userName = (string?)ViewBag.User;
            
            var movie = _movieService.Get(id);

            if (movie == null || movie.UserId != userId)
            {
                _logger.LogWarning("Unauthorized movie edit attempt for MovieTitle {Id}.", id);

                return Unauthorized();
            }
            
            var _userId = (string?)ViewBag.UserId;
            _logger.LogInformation("User {UserName} opened Edit page for Movie {Title}.", userName, movie.Title);

            return View(movie);
        }

        // POST: Edit
        [HttpPost]
        public IActionResult Edit(Movie updatedMovie)
        {
            SetUserViewBag();
            var userId = ViewBag.UserId;

            if (ViewBag.User == null)
            {
                _logger.LogWarning("Unauthorized attempt to POST edit a movie.");

                return RedirectToAction("Index", "Login");
            }
            
            var userName = (string?)ViewBag.User;

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(updatedMovie.Id))
                {
                    _logger.LogWarning("Edit failed — missing movie ID.");

                    ModelState.AddModelError("", "Movie ID is missing.");
                    return View(updatedMovie);
                }

                var movieInDb = _movieService.Get(updatedMovie.Id);

                if (movieInDb == null || movieInDb.UserId != userId)
                {
                    _logger.LogWarning("User {UserName} attempted unauthorized edit on MovieId {Id}.", userName, updatedMovie.Id);

                    return Unauthorized();
                }
                    
                updatedMovie.UserId = movieInDb.UserId;

                _logger.LogInformation("User {UserName} edited Movie {Title}.", userName, updatedMovie.Title);

                _movieService.Update(updatedMovie.Id!, updatedMovie);
                return RedirectToAction("Index", "Movies");
            }

            _logger.LogWarning("Invalid model state when user {UserName} attempted to edit movie.", userName);

            return View(updatedMovie);
        }

        // GET: Delete
        [HttpGet]
        public IActionResult Delete(string id)
        {
            SetUserViewBag();
            var userId = ViewBag.UserId;

            if (ViewBag.User == null)
            {
                _logger.LogWarning("Unauthorized attempt to access Delete page.");

                return RedirectToAction("Index", "Login");
            } 

            var userName = (string?)ViewBag.User;
            var movie = _movieService.Get(id);

            if (movie == null || movie.UserId != userId)
            {
                _logger.LogWarning("Unauthorized delete access attempt on MovieId {Id}.", id);

                return Unauthorized();
            }
                    
            _logger.LogInformation(
                "User {UserName} opened Delete page for Movie {Title}.", userName, movie.Title);

            return View(movie);
        }

        // POST: DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            SetUserViewBag();
            var userId = ViewBag.UserId;

            if (ViewBag.User == null)
            {
                _logger.LogWarning("Unauthorized POST attempt to delete movie.");

                return RedirectToAction("Index", "Login");
            } 

            var userName = (string?)ViewBag.User;
            var movie = _movieService.Get(id);

            if (movie == null || movie.UserId != userId)
            {
                _logger.LogWarning("User {UserName} attempted unauthorized delete on MovieId {Id}.", userName, id);

                return Unauthorized();
            }
                
            _logger.LogInformation("User {UserName} deleted Movie {Title}.", userName, movie.Title);
            
            _movieService.Remove(id);

            return RedirectToAction("Index", "Movies");
            
        }
        
    }
}
