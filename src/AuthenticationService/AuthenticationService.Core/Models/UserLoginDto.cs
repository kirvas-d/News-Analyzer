namespace AuthenticationService.Core.Models;

public record UserLoginDto(
    string UserName,
    string Password);