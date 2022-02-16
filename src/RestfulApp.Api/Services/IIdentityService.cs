using RestfulApp.Api.Domain;

namespace RestfulApp.Api.Services;

public interface IIdentityService
{
    Task<AuthenticationResult> LoginAsync(string email, string password);
    Task<AuthenticationResult> RegisterAsync(string email, string password);
    Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    string GetUserId();
}