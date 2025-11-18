using Microsoft.EntityFrameworkCore;

namespace MovieCatalog.Models
{
    public class MovieCatalogContext : DbContext
    {
        public MovieCatalogContext(DbContextOptions<MovieCatalogContext> options)

            : base(options) { }
        public DbSet<Movie> Movies { get; set; }
    }
}