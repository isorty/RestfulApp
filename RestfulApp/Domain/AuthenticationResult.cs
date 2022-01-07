namespace RestfulApp.Domain;

public class AuthenticationResult
{
    public string Token { get; set; }
    public bool IsAuthenticated { get; set; }
    public IEnumerable<string> Errors { get; set; }
}