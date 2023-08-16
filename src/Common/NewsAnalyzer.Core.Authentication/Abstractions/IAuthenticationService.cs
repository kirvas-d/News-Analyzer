using NewsAnalyzer.Core.Authentication.Models;

namespace NewsAnalyzer.Core.Authentication.Abstractions;

public interface IAuthenticationService
{
    Task<UserRegistrationResult> RegisterUserAsync(UserRegistrationDto userRegistrationDto);
    Task<bool> ValidateUserAsync(UserLoginDto userLoginDto);
    Task<string> CreateTokenAsync(UserLoginDto userLoginDto);
}
