namespace MovieCatalog.Models
{
    public class User
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        // Optional: add roles/claims later if needed
    }
}