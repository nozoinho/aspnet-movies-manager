using System.IdentityModel.Tokens.Jwt;
using Xunit;
using MovieCatalog.Models;
using MovieCatalog.Services;
using Microsoft.Extensions.Configuration;

public class JwtServiceTests
{
    [Fact]
    public void GenerateToken_ReturnsTokenWithUsernameClaim()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:Key", "supersecretkey123456789012345678"}, // 32 chars
            {"Jwt:Issuer", "TestIssuer"},
            {"Jwt:Audience", "TestAudience"}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var jwtService = new JwtService(configuration);
        var user = new User { Id = "1", Username = "testuser" };

        // Act
        var token = jwtService.GenerateToken(user);

        // Decode JWT to check claims
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        // Assert
        Assert.NotNull(token);
        Assert.False(string.IsNullOrEmpty(token));
        Assert.Contains(jwt.Claims, c => c.Type == System.Security.Claims.ClaimTypes.Name && c.Value == "testuser");
        Assert.Contains(jwt.Claims, c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier && c.Value == "1");
    }
}
