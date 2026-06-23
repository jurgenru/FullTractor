using System.Runtime.InteropServices.Marshalling;
using System.Security.Claims;
using System.Text;
using FullTractor.Application.Interfaces;
using FullTractor.Domain.Entities;
using FullTractor.Infrastructure.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace FullTractor.Infrastructure.Auth;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    public TokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    public (string token, DateTime expiryTime) CreateToken(User user)
    {
        DateTime expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

        List<Claim> claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Name, $"{user.Name} {user.LastName}"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        SigningCredentials signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
        {
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
            Expires = expiresAt,
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = signingCredentials
        };

        JsonWebTokenHandler handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(descriptor);

        return (token, expiresAt);
    }
}