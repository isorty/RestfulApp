using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestfulApp.Api.Data;
using RestfulApp.Api.Data.Models;
using RestfulApp.Api.Domain;
using RestfulApp.Api.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestfulApp.Api.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSecuritySettings _jwtSettings;
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(UserManager<IdentityUser> userManager, JwtSecuritySettings jwtSettings, DataContext dataContext, IMapper mapper, IHttpContextAccessor httpContext)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
        _dataContext = dataContext;
        _mapper = mapper;
        _httpContextAccessor = httpContext;
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

        if (!long.TryParse(claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Exp), out long unixTimeSeconds))
        {
            return new() { Errors = new[] { $"'{JwtRegisteredClaimNames.Exp}' claim has wrong format." } };
        }

        var expiredOnUtc = DateTimeOffset.FromUnixTimeSeconds(unixTimeSeconds);

        if (_jwtSettings.JwtOptions.IsEarlyRefreshDenied && expiredOnUtc > DateTime.UtcNow)
        {
            return new() { Errors = new[] { "Token has not expired yet." } };
        }

        var jti = claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Jti);

        var storedRefreshTokenDto = await _dataContext.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token.Equals(refreshToken));

        var storedRefreshToken = _mapper.Map<RefreshToken>(storedRefreshTokenDto);

        var storedRefreshTokenErrors = CheckRefreshToken(storedRefreshToken, jti);

        if (storedRefreshTokenErrors.Any())
        {
            return new() { Errors = storedRefreshTokenErrors };
        }

        storedRefreshTokenDto.Used = true;

        _dataContext.RefreshTokens.Update(storedRefreshTokenDto);
        await _dataContext.SaveChangesAsync();

        var user = await _userManager.FindByIdAsync(claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Sub));

        return await GenerateAuthenticationResultAsync(user);
    }

    public string GetUserId() =>
        _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

    private List<string> CheckRefreshToken(RefreshToken refreshToken, string jti)
    {
        var errors = new List<string>();

        if (refreshToken is null)
        {
            errors.Add("This refresh token does not exist.");
        }

        if (DateTime.UtcNow > refreshToken.ExpiredOn)
        {
            errors.Add("This refresh token has expired.");
        }

        if (refreshToken.Invalidated)
        {
            errors.Add("This refresh token has been invalidated.");
        }

        if (refreshToken.Used)
        {
            errors.Add("This refresh token has been used.");
        }

        if (!refreshToken.JwtId.Equals(jti))
        {
            errors.Add("This refresh token does not match this JWT.");
        }

        return errors;
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

    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken) =>
        validatedToken is JwtSecurityToken jwtSecurityToken &&
        jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

    private async Task<AuthenticationResult> GenerateAuthenticationResultAsync(IdentityUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_jwtSettings.JwtOptions.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = CreateUserClaimsIdentity(user),
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

    private ClaimsIdentity CreateUserClaimsIdentity(IdentityUser user) =>
        new(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        });
}