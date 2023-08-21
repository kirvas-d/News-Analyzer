namespace AuthenticationService.Core.Models;

public record UserRegistrationResult(
    bool Succeeded,
    IEnumerable<string> Errors);