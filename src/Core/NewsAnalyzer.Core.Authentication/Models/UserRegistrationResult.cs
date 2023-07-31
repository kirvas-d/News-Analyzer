namespace NewsAnalyzer.Core.Authentication.Models;

public record UserRegistrationResult(
    bool Succeeded,
    IEnumerable<string> Errors);
