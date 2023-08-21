namespace NewsAnalyzer.Infrastructure.IdentityAuthenticationService.Models;

public class IdentityAuthenticationServiceConfiguration
{
    public string Issuer { get; init; }

    public string Audience { get; init; }

    public string Secret { get; init; }
}
