using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Models;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]

public class MoviesApiController : ControllerBase
{

    private readonly MovieCatalogContext _context;

    public MoviesApiController(MovieCatalogContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Movie>> GetMovies()
    {
        var movies = _context.Movies.ToList();
        return Ok(movies);
    }

    [HttpGet("{id}")]

    public ActionResult<Movie> GetMovie(int id)
    {
        var movie = _context.Movies.Find(id);
        return movie == null ? NotFound() : Ok(movie);
    }

    [HttpPost]
    public ActionResult<Movie> PostMovie(Movie movie)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Movies.Add(movie);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, Movie movie)
    {
        if (id != movie.Id)
            return BadRequest("Movie ID mismatch");

        var existing = _context.Movies.Find(id);
        if (existing == null) return NotFound();

        existing.Title = movie.Title;
        existing.Director = movie.Director;
        existing.Genre = movie.Genre;
        existing.Year = movie.Year;
        existing.Rating = movie.Rating;

        _context.SaveChanges();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var movie = _context.Movies.Find(id);
         if (movie == null) return NotFound();

        _context.Movies.Remove(movie);
        _context.SaveChanges();  

        return NoContent();
    }

}