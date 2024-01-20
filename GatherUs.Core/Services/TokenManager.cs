using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GatherUs.Core.Constants;
using GatherUs.Core.Services.Interfaces;
using GatherUs.DAL.Models;
using Microsoft.IdentityModel.Tokens;

namespace GatherUs.Core.Services;

public class TokenManager: ITokenManager
{
    private static int JwtLifeTimeMinutes => 55;

    public string GenerateJwt(User user)
    {
        List<Claim> claims = new()
        {
            new Claim(GatherUsClaims.Id, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(GatherUsClaims.Email, user.Mail ?? string.Empty),
            new Claim(GatherUsClaims.Name, $"{user.FirstName ?? string.Empty} {user.LastName ?? string.Empty}"),
            new Claim(ClaimTypes.Role, user.UserType.ToString()),
        };
        
        return GetToken(claims);
    }

    private static string GetToken(IEnumerable<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConstants.Salt));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = AppConstants.JwtIssuer,
            Audience = AppConstants.JwtAudience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(JwtLifeTimeMinutes),
            SigningCredentials = credentials,
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}