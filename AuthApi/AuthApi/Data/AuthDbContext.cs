using AuthApi.Features.Games.Models;
using AuthApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Data;

public class AuthDbContext(DbContextOptions<AuthDbContext> options)
    : IdentityDbContext<User, Role, string>(options)
{
    public DbSet<Game> Games => Set<Game>();
}