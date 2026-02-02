using GameStore.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GamesEndpointsTests;

public class GameStoreFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the real database
            services.RemoveAll<DbContextOptions<GameStoreContext>>();

            // Replace with in-memory SQLite for testing
            services.AddSqlite<GameStoreContext>("Data Source=:memory:");
        });
    }
}