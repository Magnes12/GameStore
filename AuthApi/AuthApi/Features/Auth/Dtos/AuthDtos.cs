using System.ComponentModel.DataAnnotations;

namespace AuthApi.Dtos;

public record RegisterDto(
    [property: Required(AllowEmptyStrings = false)]
    [property: StringLength(50)] string FirstName,

    [property: Required(AllowEmptyStrings = false)]
    [property: StringLength(50)] string LastName,

    [property: Required(AllowEmptyStrings = false)]
    [property: EmailAddress] string Email,

    [property: Required(AllowEmptyStrings = false)]
    [property: StringLength(20, MinimumLength = 3)] string Username,

    [property: Required(AllowEmptyStrings = false)]
    [property: StringLength(100, MinimumLength = 6)] string Password
);

public record LoginDto(
    [property: Required(AllowEmptyStrings = false)]
    [property: EmailAddress] string Email,

    [property: Required(AllowEmptyStrings = false)] string Password
);

public record AuthResponseDto(
    string Token,
    string Username,
    string Email,
    List<string> Roles
);

public record UserProfileDto(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    List<string> Roles,
    DateTime CreatedAt
);