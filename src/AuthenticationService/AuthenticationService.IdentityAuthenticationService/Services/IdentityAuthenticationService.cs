namespace AuthenticationService.IdentityAuthenticationService.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationService.Core.Abstractions;
using AuthenticationService.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NewsAnalyzer.Infrastructure.IdentityAuthenticationService.Models;

public class IdentityAuthenticationService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IdentityAuthenticationServiceConfiguration _configuration;

    public IdentityAuthenticationService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IdentityAuthenticationServiceConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<UserRegistrationResult> RegisterUserAsync(UserRegistrationDto userRegistrationDto)
    {
        var userExists = await _userManager.FindByNameAsync(userRegistrationDto.UserName);
        if (userExists != null)
            return new UserRegistrationResult(false, new List<string> { "User already exists!" });

        IdentityUser user = new()
        {
            Email = userRegistrationDto.Email,
            UserName = userRegistrationDto.UserName,
        };
        var result = await _userManager.CreateAsync(user, userRegistrationDto.Password);
        if (!result.Succeeded)
            return new UserRegistrationResult(false, new List<string> { "User creation failed! Please check user details and try again." });

        await CheckAndAddAdminRoleForFirstUser(user);

        return new UserRegistrationResult(true, new List<string> { "User created successfully!" });
    }

    public async Task<bool> ValidateUserAsync(UserLoginDto userLoginDto)
    {
        var user = await _userManager.FindByNameAsync(userLoginDto.UserName);

        if (user != null && await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
        {
            var authClaims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName) };

            var userRoles = await _userManager.GetRolesAsync(user);
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = GetToken(authClaims);

            return true;
        }

        return false;
    }

    public async Task<string> CreateTokenAsync(UserLoginDto userLoginDto)
    {
        var user = await _userManager.FindByNameAsync(userLoginDto.UserName);

        var authClaims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName) };

        var userRoles = await _userManager.GetRolesAsync(user);
        authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = GetToken(authClaims);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task CheckAndAddAdminRoleForFirstUser(IdentityUser user)
    {
        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

        await _userManager.AddToRoleAsync(user, UserRoles.Admin);
        await _userManager.AddToRoleAsync(user, UserRoles.User);
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            expires: DateTime.Now.AddHours(6),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        return token;
    }
}
