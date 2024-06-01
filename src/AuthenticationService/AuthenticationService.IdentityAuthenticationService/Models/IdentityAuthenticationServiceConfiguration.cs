namespace NewsAnalyzer.Infrastructure.IdentityAuthenticationService.Models;

public class IdentityAuthenticationServiceConfiguration
{
    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    public string Secret { get; init; } = string.Empty;
}
