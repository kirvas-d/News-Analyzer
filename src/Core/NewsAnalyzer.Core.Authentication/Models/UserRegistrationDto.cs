namespace NewsAnalyzer.Core.Authentication.Models;

public record UserRegistrationDto(
    string UserName,
    string Password,
    string Email);
