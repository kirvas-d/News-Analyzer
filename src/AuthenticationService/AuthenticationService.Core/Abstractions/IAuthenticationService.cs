namespace AuthenticationService.Core.Abstractions;

using AuthenticationService.Core.Models;

public interface IAuthenticationService
{
    Task<UserRegistrationResult> RegisterUserAsync(UserRegistrationDto userRegistrationDto);
    Task<bool> ValidateUserAsync(UserLoginDto userLoginDto);
    Task<string> CreateTokenAsync(UserLoginDto userLoginDto);
}
