using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.Models
{
    public class Movie
    {
        public int Id { get; set; } 

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Director { get; set; } = string.Empty;

        [Required]
        public string Genre { get; set; } = string.Empty;

        public int Year { get; set; }

        public double Rating { get; set; }
    }
}
