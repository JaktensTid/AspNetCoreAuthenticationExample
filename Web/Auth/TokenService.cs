using System.Security.Claims;
using System.Text;
using AuthenticationExample.Infrastructure.Models;
using AuthenticationExample.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationExample.Web.Auth;

public class TokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly SymmetricSecurityKey _key;
    private readonly UserManager<User> _userManager;

    public TokenService(IOptions<JwtSettings> settings, UserManager<User> userManager)
    {
        _userManager = userManager;
        _jwtSettings = settings.Value;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
    }
    public async Task<string> CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.UniqueName, user.Id.ToString())
        };
            
        claims.AddRange(await _userManager.GetClaimsAsync(user));

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = creds,
            Issuer = _jwtSettings.ValidIssuer,
            Audience = _jwtSettings.ValidAudience
        };

        var tokenHandler = new JsonWebTokenHandler();

        return tokenHandler.CreateToken(tokenDescriptor);
    }
}