using Xunit;
using System.Collections.Generic;
using System.Linq;
using MovieCatalog.Models;

// Fake MovieService para pruebas
public class MovieServiceFake
{
    private List<Movie> _movies;

    public MovieServiceFake(List<Movie> movies)
    {
        _movies = movies;
    }

    public List<Movie> GetByUser(string userId) => _movies.Where(m => m.UserId == userId).ToList();
    public List<Movie> GetAll() => _movies.ToList();
    public Movie Get(string id) => _movies.FirstOrDefault(m => m.Id == id);
    public Movie Create(Movie movie)
    {
        _movies.Add(movie);
        return movie;
    }
    public void Update(string id, Movie movieIn)
    {
        var movie = _movies.FirstOrDefault(m => m.Id == id);
        if (movie != null)
        {
            movie.Title = movieIn.Title;
            movie.Director = movieIn.Director;
            movie.Genre = movieIn.Genre;
            movie.Year = movieIn.Year;
            movie.Rating = movieIn.Rating;
        }
    }
    public void Remove(string id) => _movies.RemoveAll(m => m.Id == id);
}

public class MovieServiceTests
{
    private List<Movie> _movies;
    private MovieServiceFake _service;

    public MovieServiceTests()
    {
        _movies = new List<Movie>();
        _service = new MovieServiceFake(_movies);
    }

    [Fact]
    public void CreateMovie_ShouldAddMovie()
    {
        var movie = new Movie { Id = "1", Title = "Inception", UserId = "user1" };
        _service.Create(movie);
        Assert.Single(_movies);
        Assert.Equal("Inception", _movies[0].Title);
    }

    [Fact]
    public void GetMovieById_ShouldReturnCorrectMovie()
    {
        var movie = new Movie { Id = "2", Title = "Matrix" };
        _service.Create(movie);
        var result = _service.Get("2");
        Assert.NotNull(result);
        Assert.Equal("Matrix", result.Title);
    }

    [Fact]
    public void GetByUser_ShouldReturnOnlyUserMovies()
    {
        _service.Create(new Movie { Id = "3", Title = "Movie1", UserId = "userA" });
        _service.Create(new Movie { Id = "4", Title = "Movie2", UserId = "userB" });
        var result = _service.GetByUser("userA");
        Assert.Single(result);
        Assert.Equal("Movie1", result[0].Title);
    }

    [Fact]
    public void UpdateMovie_ShouldModifyMovie()
    {
        var movie = new Movie { Id = "5", Title = "Old Title" };
        _service.Create(movie);
        _service.Update("5", new Movie { Title = "New Title", Director = "Dir" });
        var updated = _service.Get("5");
        Assert.Equal("New Title", updated.Title);
        Assert.Equal("Dir", updated.Director);
    }

    [Fact]
    public void RemoveMovie_ShouldDeleteMovie()
    {
        var movie = new Movie { Id = "6", Title = "ToDelete" };
        _service.Create(movie);
        _service.Remove("6");
        Assert.Empty(_movies);
    }

    [Fact]
    public void GetAllMovies_ShouldReturnAllMovies()
    {
        _service.Create(new Movie { Id = "7", Title = "M1" });
        _service.Create(new Movie { Id = "8", Title = "M2" });
        var allMovies = _service.GetAll();
        Assert.Equal(2, allMovies.Count);
    }

    [Fact]
    public void UpdateNonExistingMovie_ShouldNotThrow()
    {
        var exception = Record.Exception(() => _service.Update("999", new Movie { Title = "DoesNotExist" }));
        Assert.Null(exception);
    }
}
