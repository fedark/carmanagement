using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Services;

public class JwtValidationOptions
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string SecretKey { get; set; } = default!;

    public SecurityKey GetSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
}
