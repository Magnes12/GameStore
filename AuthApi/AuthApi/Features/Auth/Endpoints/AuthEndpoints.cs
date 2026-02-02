using AuthApi.Dtos;
using AuthApi.Models;
using AuthApi.Services;
using Microsoft.AspNetCore.Identity;

namespace AuthApi.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/register", Register);
        group.MapPost("/login", Login);
        group.MapGet("/profile", GetProfile).RequireAuthorization();
    }

    // POST /api/auth/register
    static async Task<IResult> Register(
        RegisterDto registerDto,
        UserManager<User> userManager,
        TokenService tokenService)
    {
        var existingUser = await userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser is not null)
        {
            return Results.BadRequest("Email already registered");
        }

        var user = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Username
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        await userManager.AddToRoleAsync(user, "User");

        var token = await tokenService.GenerateTokenAsync(user);

        return Results.Ok(new AuthResponseDto(
            token,
            user.UserName!,
            user.Email!,
            new List<string> { "User" }
        ));
    }

    // POST /api/auth/login
    static async Task<IResult> Login(
        LoginDto loginDto,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        TokenService tokenService)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        var passwordValid = await signInManager.CanSignInAsync(user) &&
                            await userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!passwordValid)
        {
            return Results.Unauthorized();
        }

        var roles = await userManager.GetRolesAsync(user);
        var token = await tokenService.GenerateTokenAsync(user);

        return Results.Ok(new AuthResponseDto(
            token,
            user.UserName!,
            user.Email!,
            roles.ToList()
        ));
    }

    // GET /api/auth/profile
    static async Task<IResult> GetProfile(
        HttpContext httpContext,
        UserManager<User> userManager)
    {
        var userId = httpContext.User.FindFirst(
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

        var user = await userManager.FindByIdAsync(userId!);
        if (user is null)
        {
            return Results.NotFound();
        }

        var roles = await userManager.GetRolesAsync(user);

        return Results.Ok(new UserProfileDto(
            user.UserName!,
            user.Email!,
            user.FirstName!,
            user.LastName!,
            roles.ToList(),
            user.CreatedAt
        ));
    }
}