using CentralDeUsuarios.Domain.Entities;

namespace CentralDeUsuarios.Domain.Interfaces.Security;

public interface IAuthorizationSecurity
{
    string CreateToken(Usuario usuario);
}
