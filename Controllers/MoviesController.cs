using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Models;

namespace MovieCatalog.Controllers
{
    public class MoviesController : Controller
    {
        // Simulated in-memory list
        private static List<Movie> movies = new List<Movie>()
        {
            new Movie {Id = 1, Title = "Alien", Director = "Ridley Scott", Genre = "Sci-Fi", Year = 1979, Rating = 8.5},
            new Movie {Id = 2, Title = "Forrest Gump", Director = "Robert Zemeckis", Genre = "Drama", Year = 1994, Rating = 8.8},
            new Movie {Id = 3, Title = "Back to the Future", Director = "Robert Zemeckis", Genre = "Sci-Fi", Year = 1985, Rating = 8.5},
            new Movie {Id = 4, Title = "Gladiator", Director = "Ridley Scott", Genre = "Action", Year = 2000, Rating = 8.5},
            new Movie {Id = 5, Title = "The Dark Knight", Director = "Christopher Nolan", Genre = "Action", Year = 2008, Rating = 9.0},
            new Movie {Id = 6, Title = "The Silence of the Lambs", Director = "Jonathan Demme", Genre = "Thriller", Year = 1991, Rating = 8.6},
            new Movie {Id = 7, Title = "The Terminator", Director = "James Cameron", Genre = "Sci-Fi", Year = 1984, Rating = 8.1},
            new Movie {Id = 8, Title = "The Bourne Identity", Director = "Doug Liman", Genre = "Action", Year = 2002, Rating = 8.0},
            new Movie {Id = 9, Title = "Saving Private Ryan", Director = "Steven Spielberg", Genre = "War", Year = 1998, Rating = 8.6},
            new Movie {Id = 10, Title = "The Godfather", Director = "Francis Ford Coppola", Genre = "Crime", Year = 1972, Rating = 9.2}
        };

        private static int nextId = 11;

        public IActionResult Index()
        {
            return View(movies);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                movie.Id = nextId++;
                movies.Add(movie);
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        public IActionResult Edit(int id)
        {
            var movie = movies.FirstOrDefault(m => m.Id == id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost]
        public IActionResult Edit(Movie updatedMovie)
        {
            var movie = movies.FirstOrDefault(m => m.Id == updatedMovie.Id);
            if (movie == null) return NotFound();

            if (ModelState.IsValid)
            {
                movie.Title = updatedMovie.Title;
                movie.Director = updatedMovie.Director;
                movie.Genre = updatedMovie.Genre;
                movie.Year = updatedMovie.Year;
                movie.Rating = updatedMovie.Rating;
                return RedirectToAction("Index");
            }
            return View(updatedMovie);
        }

        public IActionResult Delete(int id)
        {
            var movie = movies.FirstOrDefault(m => m.Id == id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = movies.FirstOrDefault(m => m.Id == id);
            if (movie != null) movies.Remove(movie);
            return RedirectToAction("Index");
            
        }
        
    }
}
