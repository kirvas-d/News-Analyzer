namespace AuthenticationService.Core.Models;

public record UserRegistrationDto(
    string UserName,
    string Password,
    string Email);