using System.ComponentModel.DataAnnotations;
using AuthApi.Models;

namespace AuthApi.Features.Games.Dtos;

public record CreateGameDto(
    [property: Required(AllowEmptyStrings = false)]
    [property: StringLength(50)] string Name,

    [property: Required(AllowEmptyStrings = false)]
    [property: StringLength(50)] string Genre,

    [property: Required]
    [property: Range(0.01, 9999.99)] decimal Price,

    [property: Required] DateOnly ReleaseDate
);

public record UpdateGameDto(
    [property: Required(AllowEmptyStrings = false)]
    [property: StringLength(50)] string Name,

    [property: Required(AllowEmptyStrings = false)]
    [property: StringLength(50)] string Genre,

    [property: Required]
    [property: Range(0.01, 9999.99)] decimal Price,

    [property: Required] DateOnly ReleaseDate
);

public record GameResponseDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate,
    string CreatedBy
);