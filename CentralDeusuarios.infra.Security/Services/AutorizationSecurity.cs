using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CentralDeusuarios.infra.Security.Settings;
using CentralDeUsuarios.Domain.Entities;
using CentralDeUsuarios.Domain.Interfaces.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CentralDeusuarios.infra.Security.Services;

public class AutorizationSecurity : IAuthorizationSecurity
{
  private readonly JwtSettings _jwtSettings;

  public AutorizationSecurity(IOptions< JwtSettings > jwtSettings)
  {
    _jwtSettings = jwtSettings.Value;
  }

  public string CreateToken(Usuario usuario)
  {
    var tokenHandle = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey.ToString());

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new Claim[]{
        new Claim(ClaimTypes.Name, usuario.Email),
        new Claim(ClaimTypes.Role, "USER")
      }),

      Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationInHours),

      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
      SecurityAlgorithms.HmacSha256Signature)

    };

    var token = tokenHandle.CreateToken(tokenDescriptor);
    return tokenHandle.WriteToken(token);

  }
}
