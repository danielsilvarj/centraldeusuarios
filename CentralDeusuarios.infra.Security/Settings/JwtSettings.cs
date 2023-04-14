namespace CentralDeusuarios.infra.Security.Settings;

public class JwtSettings
{
  public Guid SecretKey { get; set; }
  public int ExpirationInHours { get; set; }
}
