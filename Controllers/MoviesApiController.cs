using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class MoviesApiController : ControllerBase
{
    private readonly MovieService _movieService;

    private readonly ILogger<MoviesApiController> _logger;

    public MoviesApiController(MovieService movieService, ILogger<MoviesApiController> logger)
    {
        _movieService = movieService;
        _logger = logger;
    }

    // Helper Token user
     private string? GetUserIdFromToken()
    {
        return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }

    private string? GetUserNameFromToken()
    {
        return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
    }

    // GET api/movies
    [HttpGet]
    public ActionResult<IEnumerable<Movie>> GetMovies()
    {
        var userId = GetUserIdFromToken();
        var userName = GetUserNameFromToken();

        if (userId == null)
        {
            _logger.LogWarning("Unauthorized movie list access attempt.");

            return Unauthorized();
        } 

        _logger.LogInformation("User '{UserName}' is retrieving their movie list.", userName);

        var movies = _movieService.GetByUser(userId);

        _logger.LogInformation("User '{UserName}' retrieved {Count} movies.", userName, movies.Count());

        return Ok(movies);
    }

    // GET api/movies/{id}
    [HttpGet("{id:length(24)}")]

    public ActionResult<Movie> GetMovie(string id)
    {
        var userId = GetUserIdFromToken();
        var userName = GetUserNameFromToken();

        if (userId == null)
        {
            _logger.LogWarning("Unauthorized GetMovie attempt for movieId={MovieId}", id);

            return Unauthorized();
        } 

        _logger.LogInformation("User '{UserName}' is requesting movie with id {MovieId}.", userName, id);

        var movie = _movieService.Get(id);

        if (movie == null || movie.UserId != userId)
        {
            _logger.LogWarning("Movie not found or unauthorized access. User '{UserName}', movieId={MovieId}", userName, id);

            return NotFound();
        }
        
        _logger.LogInformation("Movie retrieved successfully: '{Title}' by user '{UserName}'.", movie.Title, userName);

        return Ok(movie);
    }

    // POST api/movies
    [HttpPost]
    public ActionResult<Movie> PostMovie(Movie movie)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model received for movie creation.");

            return BadRequest(ModelState);
        }

        var userId = GetUserIdFromToken();
        var userName = GetUserNameFromToken();

        if (userId == null) return Unauthorized();

        movie.UserId = userId;
        _movieService.Create(movie);

        _logger.LogInformation("User '{UserName}' created a new movie: '{Title}'. MovieId={MovieId}", userName, movie.Title, movie.Id);

        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
    }

    // PUT api/movies/{id}
    [HttpPut("{id:length(24)}")]
    public IActionResult Put(string id, Movie movie)
    {
        var userId = GetUserIdFromToken();
        var userName = GetUserNameFromToken();

        if (userId == null) return Unauthorized();

        var existing = _movieService.Get(id);
        if (existing == null || existing.UserId != userId)
        {
            _logger.LogWarning("User '{UserName}' attempted to update movieId={MovieId}, but it was not found or unauthorized.", userName, id);

            return NotFound();
        } 

        movie.Id = id;
        movie.UserId = userId;

        _movieService.Update(id, movie);

        _logger.LogInformation("User '{UserName}' updated movie '{Title}' (movieId={MovieId}).", userName, movie.Title, movie.Id);

        return NoContent();
    }
    
    // DELETE api/movies/{id}
    [HttpDelete("{id:length(24)}")]
    public IActionResult Delete(string id)
    {
        var userId = GetUserIdFromToken();
        var userName = GetUserNameFromToken();

        if (userId == null) return Unauthorized();

        var movie = _movieService.Get(id);

        if (movie == null || movie.UserId != userId)
        {
            _logger.LogWarning(
                "User '{UserName}' attempted to delete movieId={MovieId}, but it was not found or unauthorized.",
                userName, id
            );

            return NotFound();
        }

        _logger.LogInformation("User '{UserName}' deleted movie '{Title}' (movieId={MovieId}).", userName, movie.Title, movie.Id);

        _movieService.Remove(id);

        return NoContent();
    }

}