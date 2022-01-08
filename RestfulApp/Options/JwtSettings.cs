namespace RestfulApp.Options;

public class JwtSettings
{
    public string Secret { get; set; }
    public TimeSpan TokenLifetime { get; set; }
    public TimeSpan RefreshTokenLifeTime { get; set; }
    public bool IsEarlyRefreshDenied { get; set; }
}