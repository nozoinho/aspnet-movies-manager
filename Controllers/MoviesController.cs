using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Models;

namespace MovieCatalog.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieCatalogContext _context;

        public MoviesController(MovieCatalogContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var movies = _context.Movies.ToList();
            return View(movies);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Movies.Add(movie);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        public IActionResult Edit(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost]
        public IActionResult Edit(Movie updatedMovie)
        {
            if (ModelState.IsValid)
            {
                _context.Movies.Update(updatedMovie);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(updatedMovie);
        }

        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
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
