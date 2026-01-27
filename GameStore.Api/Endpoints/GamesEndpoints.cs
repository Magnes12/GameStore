using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    private const string GetGameEndpointName = "GetGameById";
    
    private static readonly List<GameDto> Games =
    [
        new(1, "street figher", "fight", 19.99M, new DateOnly(1992, 7, 15)),
        new(2, "Witcher 3", "open-wrold", 29.99M, new DateOnly(2001, 7, 15)),
        new(3, "Astro Bot", "platf", 49.99M, new DateOnly(2012, 7, 15))
    ];

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");
        
        // GET /games
        group.MapGet("/", () => Games)
            .WithName("GetGames");

        // GET /games/{id}
        group.MapGet("/{id}", (int id) =>
            {
                var game = Games.Find(game => game.Id == id);
        
                return game is null ? Results.NotFound() : Results.Ok(game);
            })
            .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            // if (string.IsNullOrEmpty(newGame.Name))
            // {
            //     return Results.BadRequest("Name is required");
            // }
            
            GameDto game = new(
                Games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );

            Games.Add(game);
    
            return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game);
        });

        // PUT /games/{id}
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGames) =>
        {
            var index = Games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            Games[index] = new GameDto(
                id,
                updatedGames.Name,
                updatedGames.Genre,
                updatedGames.Price,
                updatedGames.ReleaseDate
            );
    
            return Results.NoContent();
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", (int id) =>
        {
            Games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });
    }
}