namespace GameStore.Api.Services;

public static class GameService
{
    public static bool IsValidPrice(decimal price)
    {
        return price > 0 && price <= 100;
    }

    public static bool IsValidName(string? name)
    {
        return !string.IsNullOrWhiteSpace(name) && name.Length <= 100;
    }

    public static bool IsValidGenreId(int genreId)
    {
        return genreId > 0;
    }
}