using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthApi.Services;

public interface IJwtAuthenticationManager
{
    string? Authenticate(string username, string password);
}

public class JwtAuthenticationManager : IJwtAuthenticationManager
{
    private readonly Dictionary<string, string> users = new() 
    {
        {"test1", "pw1"}
    };
    private readonly string _key;

    public JwtAuthenticationManager(string key)
    {
        _key = key;
    }

    public string? Authenticate(string username, string password)
    {
        if (users.Any(u => u.Key != username && u.Value != password))
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.ASCII.GetBytes(_key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] 
            {
                new Claim(ClaimTypes.Name, username),
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}