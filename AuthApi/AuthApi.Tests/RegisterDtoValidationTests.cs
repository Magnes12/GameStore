using AuthApi.Dtos;
using System.ComponentModel.DataAnnotations;

namespace AuthApi.Tests;

public class RegisterDtoValidationTests
{
    private static List<ValidationResult> ValidateDto(object dto)
    {
        var context = new ValidationContext(dto);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(dto, context, results, validateAllProperties: true);
        return results;
    }

    [Fact]
    public void RegisterDto_ValidData_NoErrors()
    {
        var dto = new RegisterDto("John", "Doe", "john@example.com", "johndoe", "Password123!");
        var errors = ValidateDto(dto);
        Assert.Empty(errors);
    }

    [Fact]
    public void RegisterDto_EmptyFirstName_HasError()
    {
        var dto = new RegisterDto("", "Doe", "john@example.com", "johndoe", "Password123!");
        var errors = ValidateDto(dto);
        Assert.Contains(errors, e => e.MemberNames.Contains("FirstName"));
    }

    [Fact]
    public void RegisterDto_EmptyLastName_HasError()
    {
        var dto = new RegisterDto("John", "", "john@example.com", "johndoe", "Password123!");
        var errors = ValidateDto(dto);
        Assert.Contains(errors, e => e.MemberNames.Contains("LastName"));
    }

    [Fact]
    public void RegisterDto_InvalidEmail_HasError()
    {
        var dto = new RegisterDto("John", "Doe", "not-an-email", "johndoe", "Password123!");
        var errors = ValidateDto(dto);
        Assert.Contains(errors, e => e.MemberNames.Contains("Email"));
    }

    [Fact]
    public void RegisterDto_ShortUsername_HasError()
    {
        var dto = new RegisterDto("John", "Doe", "john@example.com", "ab", "Password123!");
        var errors = ValidateDto(dto);
        Assert.Contains(errors, e => e.MemberNames.Contains("Username"));
    }

    [Fact]
    public void RegisterDto_LongUsername_HasError()
    {
        var dto = new RegisterDto("John", "Doe", "john@example.com", new string('a', 21), "Password123!");
        var errors = ValidateDto(dto);
        Assert.Contains(errors, e => e.MemberNames.Contains("Username"));
    }

    [Fact]
    public void RegisterDto_ShortPassword_HasError()
    {
        var dto = new RegisterDto("John", "Doe", "john@example.com", "johndoe", "12345");
        var errors = ValidateDto(dto);
        Assert.Contains(errors, e => e.MemberNames.Contains("Password"));
    }

    [Fact]
    public void RegisterDto_LongPassword_HasError()
    {
        var dto = new RegisterDto("John", "Doe", "john@example.com", "johndoe", new string('a', 101));
        var errors = ValidateDto(dto);
        Assert.Contains(errors, e => e.MemberNames.Contains("Password"));
    }
}