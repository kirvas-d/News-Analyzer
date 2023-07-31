namespace NewsAnalyzer.Core.Authentication.Models;

public record UserLoginDto(
    string UserName,
    string Password);
