using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using MovieCatalog.Models;
using BCrypt.Net;

namespace MovieCatalog.Services
{
    public class UserService
    {

        private readonly IMongoCollection<User> _users;

        public UserService(IOptions<MongoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<User>(settings.Value.UserCollectionName);
        }

        public List<User> GetAll() => _users.Find(u => true).ToList();

        public bool Register(User user)
        {
            if (_users.Find(u => u.Username == user.Username).Any())
                return false;

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _users.InsertOne(user);
            return true;
        }

        public User? Authenticate(string username, string password)
        {
            var user = _users.Find(u => u.Username == username).FirstOrDefault();
            if (user == null) return null;

            return BCrypt.Net.BCrypt.Verify(password, user.Password) ? user : null;
        }
    }
}