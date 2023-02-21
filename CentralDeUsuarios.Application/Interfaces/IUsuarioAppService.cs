using CentralDeUsuarios.Application.Commands;

namespace CentralDeUsuarios.Application.Interfaces;

public interface IUsuarioAppService
{
    Task CriarUsuario(CriarUsuarioCommand command);
    Task AutenticarUsuario(AutenticarUsuarioCommand command);
}
