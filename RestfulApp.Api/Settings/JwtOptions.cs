namespace RestfulApp.Api.Settings;

public class JwtOptions
{
    public string Secret { get; set; }
    public TimeSpan TokenLifetime { get; set; }
    public TimeSpan RefreshTokenLifeTime { get; set; }
    public bool IsEarlyRefreshDenied { get; set; }
}