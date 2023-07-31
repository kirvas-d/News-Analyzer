﻿using Microsoft.AspNetCore.Mvc;
using NewsAnalyzer.Application.ApiGateway.Models;
using NewsAnalyzer.Core.Authentication.Abstractions;
using NewsAnalyzer.Core.Authentication.Models;

namespace NewsAnalyzer.Application.ApiGateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService) 
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<string>> Login([FromBody]UserLoginDto model) 
    {
        if(await _authenticationService.ValidateUserAsync(model) == false)
            return BadRequest(ModelState);

        var token = await _authenticationService.CreateTokenAsync(model);
        if (token == null)
            return BadRequest(ModelState);

        return Ok(token);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody]UserRegistrationDto model)
    {
        var result = await _authenticationService.RegisterUserAsync(model);

        if(!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = result.Errors.First() });
        else
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
    }

    //[HttpPost("BearerToken")]
    //public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken(AuthenticationRequest request)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest("Bad credentials");
    //    }

    //    var user = await _userManager.FindByNameAsync(request.UserName);

    //    if (user == null)
    //    {
    //        return BadRequest("Bad credentials");
    //    }

    //    var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

    //    if (!isPasswordValid)
    //    {
    //        return BadRequest("Bad credentials");
    //    }

    //    var token = _jwtService.CreateToken(user);

    //    return Ok(token);
    //}
}
