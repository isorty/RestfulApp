using Microsoft.AspNetCore.Mvc;
using RestfulApp.Contracts.V1;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Contracts.V1.Responses;
using RestfulApp.Services;

namespace RestfulApp.Controllers.V1;

public class IdentityController : Controller
{
    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost(ApiRoutes.Identity.Login)]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginRequest userLoginRequest)
    {
        var authResponse = await _identityService.LoginAsync(userLoginRequest.Email, userLoginRequest.Password);

        if (!authResponse.IsAuthenticated)
        {
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });
        }

        return Ok(new AuthSuccessResponse { Token = authResponse.Token });
    }

    [HttpPost(ApiRoutes.Identity.Register)]
    public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationRequest userRegistrationRequest)
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

        return Ok(new AuthSuccessResponse { Token = authResponse.Token });
    }
}