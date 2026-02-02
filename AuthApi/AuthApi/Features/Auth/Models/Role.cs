using Microsoft.AspNetCore.Identity;

namespace AuthApi.Models;

public class Role : IdentityRole
{
    public string? Description { get; set; }
}