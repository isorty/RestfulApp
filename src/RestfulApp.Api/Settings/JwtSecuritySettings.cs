using Microsoft.IdentityModel.Tokens;

namespace RestfulApp.Api.Settings;

public class JwtSecuritySettings
{
    public JwtOptions JwtOptions { get; init; }
    public TokenValidationParameters TokenValidationParameters { get; init; }

    public JwtSecuritySettings() { }

    public JwtSecuritySettings(JwtOptions jwtOptions, TokenValidationParameters tokenValidationParameters)
    {
        JwtOptions = jwtOptions;
        TokenValidationParameters = tokenValidationParameters;
    }
}