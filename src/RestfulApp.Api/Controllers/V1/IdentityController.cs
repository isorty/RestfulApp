﻿using Microsoft.AspNetCore.Mvc;
using RestfulApp.Api.Contracts.V1;
using RestfulApp.Api.Contracts.V1.Requests;
using RestfulApp.Api.Contracts.V1.Responses;
using RestfulApp.Api.Services;
using System.Net.Mime;

namespace RestfulApp.Api.Controllers.V1;

[Produces(MediaTypeNames.Application.Json)]
public class IdentityController : ApiController
{
    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    /// <summary>
    /// Logins a user by its email and password.
    /// </summary>
    /// <response code="200">Login succeed.</response>
    /// <response code="400">Login failed.</response>
    [HttpPost(ApiRoutes.Identity.Login)]
    [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
    [ProducesResponseType(typeof(AuthFailedResponse), 400)]
    public async Task<IActionResult> LoginAsync(UserLoginRequest userLoginRequest)
    {
        var authResponse = await _identityService.LoginAsync(userLoginRequest.Email, userLoginRequest.Password);

        if (!authResponse.IsAuthenticated)
        {
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });
        }

        return Ok(new AuthSuccessResponse
        {
            Token = authResponse.Token,
            RefreshToken = authResponse.RefreshToken
        });
    }

    /// <summary>
    /// Registers a new user with email and password.
    /// </summary>
    /// <response code="200">Registration succeed.</response>
    /// <response code="400">Registration failed.</response>
    [HttpPost(ApiRoutes.Identity.Register)]
    [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
    [ProducesResponseType(typeof(AuthFailedResponse), 400)]
    public async Task<IActionResult> RegisterAsync(UserRegistrationRequest userRegistrationRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthFailedResponse
            {
                Errors = ModelState.Values.SelectMany(entry => entry.Errors.Select(error => error.ErrorMessage))
            });
        }

        var authResponse = await _identityService.RegisterAsync(userRegistrationRequest.Email, userRegistrationRequest.Password);

        if (!authResponse.IsAuthenticated)
        {
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });
        }

        return Ok(new AuthSuccessResponse
        {
            Token = authResponse.Token,
            RefreshToken = authResponse.RefreshToken
        });
    }

    /// <summary>
    /// Returns a new token.
    /// </summary>
    /// <response code="200">Refreshing token succeed.</response>
    /// <response code="400">Refreshing token failed.</response>
    [HttpPost(ApiRoutes.Identity.Refresh)]
    [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
    [ProducesResponseType(typeof(AuthFailedResponse), 400)]
    public async Task<IActionResult> RefreshAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var authResponse = await _identityService.RefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken);

        if (!authResponse.IsAuthenticated)
        {
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });
        }

        return Ok(new AuthSuccessResponse
        {
            Token = authResponse.Token,
            RefreshToken = authResponse.RefreshToken
        });
    }
}