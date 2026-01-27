using GameStore.Api.Dtos;

const string getGameEndpointName = "GetGameById";
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games =
[
    new(1, "street figher", "fight", 19.99M, new DateOnly(1992, 7, 15)),
    new(2, "Witcher 3", "open-wrold", 29.99M, new DateOnly(2001, 7, 15)),
    new(3, "Astro Bot", "platf", 49.99M, new DateOnly(2012, 7, 15))
];

// GET /games
app.MapGet("/games", () => games)
    .WithName("GetGames");

// GET /games/{id}
app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id))
    .WithName(getGameEndpointName);

// POST /games
app.MapPost("/games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);
    
    return Results.CreatedAtRoute(getGameEndpointName, new {id = game.Id}, game);
});

// PUT /games/{id}
app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGames) =>
{
    var index = games.FindIndex(game => game.Id == id);

    games[index] = new GameDto(
        id,
        updatedGames.Name,
        updatedGames.Genre,
        updatedGames.Price,
        updatedGames.ReleaseDate
        );
    
    return Results.NoContent();
});

// DELETE /games/{id}
app.MapDelete("/games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);
    return Results.NoContent();
});


app.Run();