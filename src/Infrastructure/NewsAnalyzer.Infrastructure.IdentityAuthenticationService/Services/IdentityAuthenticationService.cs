using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NewsAnalyzer.Core.Authentication.Abstractions;
using NewsAnalyzer.Core.Authentication.Models;
using NewsAnalyzer.Infrastructure.IdentityAuthenticationService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsAnalyzer.Infrastructure.IdentityAuthenticationService.Services;

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
            //SecurityStamp = Guid.NewGuid().ToString(),
            UserName = userRegistrationDto.UserName
        };
        var result = await _userManager.CreateAsync(user, userRegistrationDto.Password);
        if (!result.Succeeded)
            return new UserRegistrationResult(false, new List<string> { "User creation failed! Please check user details and try again." }); //StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

        await CheckAndAddAdminRoleForFirstUser(user);

        return new UserRegistrationResult(true, new List<string> { "User created successfully!" }); //Ok(new Response { Status = "Success", Message = "User created successfully!" });
    }

    public async Task<bool> ValidateUserAsync(UserLoginDto userLoginDto)
    {
        var user = await _userManager.FindByNameAsync(userLoginDto.UserName);

        if (user != null && await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
        {
            var authClaims = new List<Claim>{ new Claim(ClaimTypes.Name, user.UserName) };

            var userRoles = await _userManager.GetRolesAsync(user);
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = GetToken(authClaims);

            return true;

            //return Ok(new
            //{
            //    token = new JwtSecurityTokenHandler().WriteToken(token),
            //    expiration = token.ValidTo
            //});
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

        //if (_userManager.Users.Count() == 1)
        //{
            await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            await _userManager.AddToRoleAsync(user, UserRoles.User);
        //}
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            expires: DateTime.Now.AddHours(6),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }
}
