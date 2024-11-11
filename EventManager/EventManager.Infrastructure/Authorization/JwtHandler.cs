using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventManager.Domain.Model;
using EventManager.Infrastructure.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using EventManager.Application.Common.Interfaces;

namespace EventManager.Infrastructure.Authorization;

public class JwtHandler(IConfiguration configuration) : IJwtHandler
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateToken(ApplicationUser user, IList<string> roles)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(roles);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ??
                                 AuthorizationConstants.JWT_SECRET_KEY));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? AuthorizationConstants.ISSUER,
            audience: _configuration["Jwt:Audience"] ?? AuthorizationConstants.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["Jwt:ExpirationInMinutes"] ??
                         AuthorizationConstants.JWT_EXPIRATION_MINUTES.ToString())),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}