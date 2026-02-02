using AuthApi.Models;
using AuthApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace AuthApi.Tests;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly IConfiguration _config;

    public TokenServiceTests()
    {
        // Mock configuration
        var configDict = new Dictionary<string, string?>
        {
            { "Jwt:Secret", "TestSecretKeyThatIsLongEnoughForHmacSha256" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" },
            { "Jwt:ExpirationMinutes", "60" }
        };

        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(configDict)
            .Build();

        // Simplified UserManager mock
        var userStore = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStore.Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            Mock.Of<ILogger<UserManager<User>>>()
        );

        _tokenService = new TokenService(_config, _userManagerMock.Object);
    }

    [Fact]
    public async Task GenerateToken_ValidUser_ReturnsToken()
    {
        var user = new User
        {
            Id = "test-id-123",
            UserName = "testuser",
            Email = "test@example.com"
        };

        _userManagerMock
            .Setup(um => um.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "User" });

        var token = await _tokenService.GenerateTokenAsync(user);

        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public async Task GenerateToken_AdminUser_TokenContainsAdminRole()
    {
        var admin = new User
        {
            Id = "admin-id-123",
            UserName = "admin",
            Email = "admin@example.com"
        };

        _userManagerMock
            .Setup(um => um.GetRolesAsync(admin))
            .ReturnsAsync(new List<string> { "Admin" });

        var token = await _tokenService.GenerateTokenAsync(admin);

        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var decoded = handler.ReadJwtToken(token);

        var roleClaim = decoded.Claims.FirstOrDefault(
            c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

        Assert.NotNull(roleClaim);
        Assert.Equal("Admin", roleClaim.Value);
    }

    [Fact]
    public async Task GenerateToken_UserWithMultipleRoles_TokenContainsAllRoles()
    {
        var user = new User
        {
            Id = "multi-role-id",
            UserName = "multirole",
            Email = "multi@example.com"
        };

        _userManagerMock
            .Setup(um => um.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Admin", "User" });

        var token = await _tokenService.GenerateTokenAsync(user);

        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var decoded = handler.ReadJwtToken(token);

        var roles = decoded.Claims
            .Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
            .Select(c => c.Value)
            .ToList();

        Assert.Contains("Admin", roles);
        Assert.Contains("User", roles);
    }

    [Fact]
    public async Task GenerateToken_CheckExpiration_TokenHasCorrectExpiry()
    {
        var user = new User
        {
            Id = "expiry-id",
            UserName = "expiryuser",
            Email = "expiry@example.com"
        };

        _userManagerMock
            .Setup(um => um.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "User" });

        var token = await _tokenService.GenerateTokenAsync(user);

        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var decoded = handler.ReadJwtToken(token);

        var expiration = decoded.ValidTo;
        var expected = DateTime.UtcNow.AddMinutes(60);

        Assert.True(Math.Abs((expiration - expected).TotalSeconds) < 5);
    }

    [Fact]
    public async Task GenerateToken_CheckIssuer_TokenHasCorrectIssuer()
    {
        var user = new User
        {
            Id = "issuer-id",
            UserName = "issueruser",
            Email = "issuer@example.com"
        };

        _userManagerMock
            .Setup(um => um.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "User" });

        var token = await _tokenService.GenerateTokenAsync(user);

        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var decoded = handler.ReadJwtToken(token);

        Assert.Equal("TestIssuer", decoded.Issuer);
    }
}