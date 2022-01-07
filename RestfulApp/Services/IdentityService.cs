using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RestfulApp.Domain;
using RestfulApp.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestfulApp.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
    }

    public async Task<AuthenticationResult> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return new() { Errors = new[] { "User does not exist." } };
        }

        if (await _userManager.CheckPasswordAsync(user, password))
        {
            return GenerateAuthenticationResult(user);
        }

        return new() { Errors = new[] { "User/password combination is wrong." } };
    }

    public async Task<AuthenticationResult> RegisterAsync(string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser is not null)
        {
            return new() { Errors = new[] { "User with this email address already exists." } };
        }

        var newUser = new IdentityUser
        {
            Email = email,
            UserName = email
        };

        var createdUser = await _userManager.CreateAsync(newUser, password);

        if (!createdUser.Succeeded)
        {
            return new() { Errors = createdUser.Errors.Select(e => e.Description) };
        }

        return GenerateAuthenticationResult(newUser);
    }

    private AuthenticationResult GenerateAuthenticationResult(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new()
        {
            IsAuthenticated = true,
            Token = tokenHandler.WriteToken(token)
        };
    }
}