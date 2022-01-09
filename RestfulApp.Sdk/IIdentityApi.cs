using Refit;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Contracts.V1.Responses;

namespace RestfulApp.Sdk;

public interface IIdentityApi
{
    [Post("/api/v1/identity/register")]
    Task<ApiResponse<AuthSuccessResponse>> RegisterAsync([Body] UserRegistrationRequest userRegistrationRequest);

    [Post("/api/v1/identity/login")]
    Task<ApiResponse<AuthSuccessResponse>> LoginAsync([Body] UserLoginRequest userLoginRequest);

    [Post("/api/v1/identity/refresh")]
    Task<ApiResponse<AuthSuccessResponse>> RefreshAsync([Body] RefreshTokenRequest refreshTokenRequest);
}