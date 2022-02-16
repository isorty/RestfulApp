namespace RestfulApp.Api.Domain;

public class RefreshToken
{
    public string Token { get; set; }
    public string JwtId { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ExpiredOn { get; set; }
    public bool Used { get; set; }
    public bool Invalidated { get; set; }
    public string UserId { get; set; }
}