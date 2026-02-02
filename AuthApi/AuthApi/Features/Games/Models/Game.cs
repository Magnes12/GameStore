using AuthApi.Models;

namespace AuthApi.Features.Games.Models;

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Genre { get; set; }
    public decimal Price { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string UserId { get; set; }
    public User? User { get; set; } 
}