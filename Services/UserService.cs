using MovieCatalog.Models;
using System.Collections.Generic;
using System.Linq;

namespace MovieCatalog.Services
{
    public class UserService
    {
        // Static in-memory list of users
        private static readonly List<User> _users = new();

        public bool Register(User user)
        {
            if (_users.Any(u => u.Username == user.Username))
                return false;

            _users.Add(user);
            return true;
        }

        public User? Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u => 
                u.Username == username && u.Password == password);
        }

        public List<User> GetAll() => _users;
    }
}