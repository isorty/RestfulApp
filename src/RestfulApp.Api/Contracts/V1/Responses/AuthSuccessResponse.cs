namespace RestfulApp.Api.Contracts.V1.Responses;

public sealed class AuthSuccessResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}