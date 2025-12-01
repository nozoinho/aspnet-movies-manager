using System;
using System.Collections.Generic;
using System.Linq;
using MovieCatalog.Models;
using Xunit;
using BCrypt.Net;

namespace MovieCatalog.Tests
{
    public class UserServiceTests
    {
        private List<User> _fakeUsers;
        private UserServiceForTest _userService;

        public UserServiceTests()
        {
            // Initialize an in-memory list to simulate the database
            _fakeUsers = new List<User>
            {
                new User { Username = "user1", Password = BCrypt.Net.BCrypt.HashPassword("pass1") },
                new User { Username = "user2", Password = BCrypt.Net.BCrypt.HashPassword("pass2") }
            };

            // Use the test service with in-memory list
            _userService = new UserServiceForTest(_fakeUsers);
        }

        [Fact]
        public void GetAll_ReturnsAllUsers()
        {
            var result = _userService.GetAll();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Register_NewUser_ReturnsTrue()
        {
            var newUser = new User { Username = "user3", Password = "pass3" };
            var result = _userService.Register(newUser);

            Assert.True(result);
            Assert.Contains(_fakeUsers, u => u.Username == "user3");
        }

        [Fact]
        public void Register_ExistingUser_ReturnsFalse()
        {
            var existingUser = new User { Username = "user1", Password = "newpass" };
            var result = _userService.Register(existingUser);

            Assert.False(result);
        }

        [Fact]
        public void Authenticate_ValidUser_ReturnsUser()
        {
            var user = _userService.Authenticate("user1", "pass1");
            Assert.NotNull(user);
            Assert.Equal("user1", user.Username);
        }

        [Fact]
        public void Authenticate_InvalidPassword_ReturnsNull()
        {
            var user = _userService.Authenticate("user1", "wrongpass");
            Assert.Null(user);
        }

        [Fact]
        public void Authenticate_NonExistentUser_ReturnsNull()
        {
            var user = _userService.Authenticate("nouser", "pass");
            Assert.Null(user);
        }

        [Fact]
        public void Register_PasswordIsHashed()
        {
            var newUser = new User { Username = "user4", Password = "mypassword" };
            _userService.Register(newUser);

            var storedUser = _fakeUsers.First(u => u.Username == "user4");
            Assert.NotEqual("mypassword", storedUser.Password);
            Assert.True(BCrypt.Net.BCrypt.Verify("mypassword", storedUser.Password));
        }
    }

    // Test-specific service using only in-memory list
    public class UserServiceForTest
    {
        private readonly List<User> _users;

        public UserServiceForTest(List<User> users)
        {
            _users = users;
        }

        public List<User> GetAll() => _users;

        public bool Register(User user)
        {
            if (_users.Any(u => u.Username == user.Username))
                return false;

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _users.Add(user);
            return true;
        }

        public User? Authenticate(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user == null) return null;

            return BCrypt.Net.BCrypt.Verify(password, user.Password) ? user : null;
        }
    }
}
