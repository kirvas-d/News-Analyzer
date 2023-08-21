using AuthenticationService.Core.Models;

namespace AuthenticationService.Core.Abstractions;

public interface IAuthenticationService
{
    Task<UserRegistrationResult> RegisterUserAsync(UserRegistrationDto userRegistrationDto);
    Task<bool> ValidateUserAsync(UserLoginDto userLoginDto);
    Task<string> CreateTokenAsync(UserLoginDto userLoginDto);
}
