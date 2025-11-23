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

    public MoviesApiController(MovieService movieService)
    {
        _movieService = movieService;
    }

    // Helper Token user
     private string? GetUserIdFromToken()
    {
        return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }

    // GET api/movies
    [HttpGet]
    public ActionResult<IEnumerable<Movie>> GetMovies()
    {
        var userId = GetUserIdFromToken();
        if (userId == null) return Unauthorized();

        var movies = _movieService.GetByUser(userId);
        return Ok(movies);
    }

    // GET api/movies/{id}
    [HttpGet("{id:length(24)}")]

    public ActionResult<Movie> GetMovie(string id)
    {
        var userId = GetUserIdFromToken();
        if (userId == null) return Unauthorized();

        var movie = _movieService.Get(id);

        if (movie == null || movie.UserId != userId) 
            return NotFound();

        return Ok(movie);
    }

    // POST api/movies
    [HttpPost]
    public ActionResult<Movie> PostMovie(Movie movie)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserIdFromToken();
        if (userId == null) return Unauthorized();

        movie.UserId = userId;
        _movieService.Create(movie);

        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
    }

    // PUT api/movies/{id}
    [HttpPut("{id:length(24)}")]
    public IActionResult Put(string id, Movie movie)
    {
        var userId = GetUserIdFromToken();
        if (userId == null) return Unauthorized();

        var existing = _movieService.Get(id);
        if (existing == null || existing.UserId != userId) return NotFound();

        movie.Id = id;
        movie.UserId = userId;

        _movieService.Update(id, movie);

        return NoContent();
    }
    
    // DELETE api/movies/{id}
    [HttpDelete("{id:length(24)}")]
    public IActionResult Delete(string id)
    {
        var userId = GetUserIdFromToken();
        if (userId == null) return Unauthorized();

        var movie = _movieService.Get(id);

        if (movie == null || movie.UserId != userId) return NotFound();

        _movieService.Remove(id);

        return NoContent();
    }

}