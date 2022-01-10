using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestfulApp.Data;
using RestfulApp.Data.Models;
using RestfulApp.Domain;
using RestfulApp.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestfulApp.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings, DataContext dataContext, IMapper mapper)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
        _dataContext = dataContext;
        _mapper = mapper;
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
            return await GenerateAuthenticationResultAsync(user);
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

        return await GenerateAuthenticationResultAsync(newUser);
    }

    public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
    {
        var claimsPrincipal = GetPrincipal(token);

        if (claimsPrincipal is null)
        {
            return new() { Errors = new[] { "Invalid token." } };
        }

        var expiredOnUnix = long.Parse(claimsPrincipal.Claims.Single(claim => claim.Type.Equals(JwtRegisteredClaimNames.Exp)).Value);
        var expiredOnUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddSeconds(expiredOnUnix);

        if (_jwtSettings.JwtOptions.IsEarlyRefreshDenied && expiredOnUtc > DateTime.UtcNow)
        {
            return new() { Errors = new[] { "Token has not expired yet." } };
        }

        var jti = claimsPrincipal.Claims.Single(claim => claim.Type.Equals(JwtRegisteredClaimNames.Jti)).Value;

        var storedRefreshToken = await _dataContext.RefreshTokens
            .SingleOrDefaultAsync(rt => rt.Token.Equals(refreshToken));

        if (storedRefreshToken is null)
        {
            return new() { Errors = new[] { "This refresh token does not exist." } };
        }

        if (DateTime.UtcNow > storedRefreshToken.ExpiredOn)
        {
            return new() { Errors = new[] { "This refresh token has expired." } };
        }

        if (storedRefreshToken.Invalidated)
        {
            return new() { Errors = new[] { "This refresh token has been invalidated." } };
        }

        if (storedRefreshToken.Used)
        {
            return new() { Errors = new[] { "This refresh token has been used." } };
        }

        if (!storedRefreshToken.JwtId.Equals(jti))
        {
            return new() { Errors = new[] { "This refresh token does not match this JWT." } };
        }

        storedRefreshToken.Used = true;

        _dataContext.RefreshTokens.Update(storedRefreshToken);
        await _dataContext.SaveChangesAsync();

        var user = await _userManager.FindByIdAsync(claimsPrincipal.Claims.Single(claim => claim.Type.Equals("id")).Value);

        return await GenerateAuthenticationResultAsync(user);
    }

    private ClaimsPrincipal GetPrincipal(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var tokenValidationParameters = _jwtSettings.TokenValidationParameters.Clone();
            tokenValidationParameters.ValidateLifetime = false;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }

    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
    }

    private async Task<AuthenticationResult> GenerateAuthenticationResultAsync(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.JwtOptions.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id)
            }),
            Expires = DateTime.UtcNow.Add(_jwtSettings.JwtOptions.TokenLifetime),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var refreshToken = new RefreshToken
        {
            JwtId = token.Id,
            UserId = user.Id,
            CreationDate = DateTime.UtcNow,
            ExpiredOn = DateTime.UtcNow.Add(_jwtSettings.JwtOptions.RefreshTokenLifeTime)
        };

        var refreshTokenDto = _mapper.Map<RefreshTokenDto>(refreshToken);

        await _dataContext.RefreshTokens.AddAsync(refreshTokenDto);
        await _dataContext.SaveChangesAsync();


        return new()
        {
            IsAuthenticated = true,
            Token = tokenHandler.WriteToken(token),
            RefreshToken = refreshTokenDto.Token
        };
    }
}