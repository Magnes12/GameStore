using AuthApi.Features.Games.Dtos;
using AuthApi.Features.Games.Models;

namespace AuthApi.Features.Games.Endpoints;

public static class GamesEndpoints
{
    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/games");
        
        group.MapGet("/create", CreateGame);
        
    }
    // POST /api/games/create
    static async Task<IResult> CreateGame(CreateGameDto createGameDto)
    {
        var game = new Game
        {
            Name = createGameDto.Name,
            Genre = createGameDto.Genre,
            Price = createGameDto.Price,
            ReleaseDate = createGameDto.ReleaseDate
        };
        
        return Results.Created($"/api/games/{game.Id}", game);
    }
}