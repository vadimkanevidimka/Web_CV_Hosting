using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Buisness.Dtos.Tokens;
using AuthService.DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Buisness.Services.Algorithms;

public class TokensGenerator(
    IConfiguration configuration)
{
    public Token GenerateAccessToken(User user, IEnumerable<string> userRoles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id)
        };

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var tokenExpires = DateTime.UtcNow.AddMinutes(double.Parse(configuration["JwtSettings:AccessTokenExpirationMinutes"]!));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:AccessTokenSecret"]!));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: tokenExpires,
            issuer: configuration["JwtSettings:Issuer"]!,
            signingCredentials: signingCredentials);

        return new Token
        {
            Value = new JwtSecurityTokenHandler().WriteToken(securityToken),
            ExpiresIn = tokenExpires
        };
    }

    public Token GenerateRefreshToken()
    {
        return new Token
        {
            Value = Guid.NewGuid().ToString(),
            ExpiresIn = DateTime.UtcNow.AddMinutes(GetJwtSetting<double>("RefreshTokenExpiresInMinutes"))
        };
    }

    private T GetJwtSetting<T>(string key) => configuration.GetValue<T>($"JwtSettings:{key}");
}