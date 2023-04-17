namespace CentralDeusuarios.infra.Security.Settings;

public class JwtSettings
{
  public string SecretKey { get; set; }
  public int ExpirationInHours { get; set; }
}
