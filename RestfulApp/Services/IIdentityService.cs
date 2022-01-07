using RestfulApp.Domain;

namespace RestfulApp.Services;

public interface IIdentityService
{
    Task<AuthenticationResult> LoginAsync(string email, string password);
    Task<AuthenticationResult> RegisterAsync(string email, string password);    
}