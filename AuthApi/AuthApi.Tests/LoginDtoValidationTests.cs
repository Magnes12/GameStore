using AuthApi.Dtos;
using System.ComponentModel.DataAnnotations;

namespace AuthApi.Tests;

public class LoginDtoValidationTests
{
    private static List<ValidationResult> ValidateDto(object dto)
    {
        var context = new ValidationContext(dto);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(dto, context, results, validateAllProperties: true);
        return results;
    }

    [Fact]
    public void LoginDto_ValidData_NoErrors()
    {
        var dto = new LoginDto("john@example.com", "Password123!");
        var errors = ValidateDto(dto);
        Assert.Empty(errors);
    }

    [Fact]
    public void LoginDto_InvalidEmail_HasError()
    {
        var dto = new LoginDto("not-an-email", "Password123!");
        var errors = ValidateDto(dto);
        Assert.Contains(errors, e => e.MemberNames.Contains("Email"));
    }

    [Fact]
    public void LoginDto_EmptyEmail_HasError()
    {
        var dto = new LoginDto("", "Password123!");
        var errors = ValidateDto(dto);
        Assert.Contains(errors, e => e.MemberNames.Contains("Email"));
    }

    [Fact]
    public void LoginDto_EmptyPassword_HasError()
    {
        var dto = new LoginDto("john@example.com", "");
        var errors = ValidateDto(dto);
        Assert.Contains(errors, e => e.MemberNames.Contains("Password"));
    }
    
}