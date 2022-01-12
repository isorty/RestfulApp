using Microsoft.IdentityModel.Tokens;

namespace RestfulApp.Api.Settings;

public class JwtSettings
{
    public JwtOptions JwtOptions { get; init; }
    public TokenValidationParameters TokenValidationParameters { get; init; }

    public JwtSettings()
    {

    }

    public JwtSettings(JwtOptions jwtOptions, TokenValidationParameters tokenValidationParameters)
    {
        JwtOptions = jwtOptions;
        TokenValidationParameters = tokenValidationParameters;
    }
}