using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Services;

public class JwtService
{
    private readonly JwtValidationOptions jwtOptions_;

    public JwtService(IOptions<JwtValidationOptions> jwtOptions)
    {
        jwtOptions_ = jwtOptions.Value;
    }

    public string GetToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name)
        };
        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));

        var jwt = new JwtSecurityToken(
            issuer: jwtOptions_.Issuer,
            audience: jwtOptions_.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new(jwtOptions_.GetSecurityKey(), SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }
}
