using Refit;
using RestfulApp.Contracts.V1;
using RestfulApp.Contracts.V1.Requests;
using RestfulApp.Contracts.V1.Responses;

namespace RestfulApp.Sdk;

public interface IIdentityApi
{
    [Post($"/{ApiRoutes.Identity.Register}")]
    Task<ApiResponse<AuthSuccessResponse>> RegisterAsync([Body] UserRegistrationRequest userRegistrationRequest);

    [Post($"/{ApiRoutes.Identity.Login}")]
    Task<ApiResponse<AuthSuccessResponse>> LoginAsync([Body] UserLoginRequest userLoginRequest);

    [Post($"/{ApiRoutes.Identity.Refresh}")]
    Task<ApiResponse<AuthSuccessResponse>> RefreshAsync([Body] RefreshTokenRequest refreshTokenRequest);
}